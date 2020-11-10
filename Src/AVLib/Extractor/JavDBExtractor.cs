// JavDBExtractor.cs 2020

namespace AVCli.AVLib.Extractor
{
    using AngleSharp;
    using AngleSharp.Dom;
    using AngleSharp.Html.Dom.Events;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="JavDBExtractor" />.
    /// </summary>
    public class JavDBExtractor : IExtractor
    {
        /// <summary>
        /// Defines the cacheProvider.
        /// </summary>
        private readonly ICacheProvider cacheProvider;

        /// <summary>
        /// Defines the conf.
        /// </summary>
        private readonly AVLib.Configuration conf;

        /// <summary>
        /// Defines the baseUrl.
        /// </summary>
        private readonly string baseUrl;

        /// <summary>
        /// Defines the context.
        /// </summary>
        private readonly IBrowsingContext context;

        /// <summary>
        /// Defines the htmlContentReader.
        /// </summary>
        private readonly IHtmlContentReader htmlContentReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="JavDBExtractor"/> class.
        /// </summary>
        /// <param name="htmlContentReader">The htmlContentReader<see cref="IHtmlContentReader"/>.</param>
        /// <param name="cacheProvider">The cacheProvider<see cref="ICacheProvider"/>.</param>
        /// <param name="configuration">The configuration<see cref="Configuration"/>.</param>
        public JavDBExtractor(IHtmlContentReader htmlContentReader, ICacheProvider cacheProvider, AVLib.Configuration configuration)
        {
            this.htmlContentReader = htmlContentReader;
            this.cacheProvider = cacheProvider;
            this.conf = configuration;
            this.baseUrl = configuration.GetBaseUrl(GetKey());
            this.context = BrowsingContext.New(Configuration.Default);
        }

        /// <summary>
        /// The GetDataAsync.
        /// </summary>
        /// <param name="number">The number<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{AvData}"/>.</returns>
        public async Task<AvData> GetDataAsync(string number)
        {
            var data = await this.cacheProvider.GetDataAsync(number);
            if (data != null)
            {
                return data;
            }
            data = await this.GetByHtmlAsync(number);
            return data;
        }

        /// <summary>
        /// The GetDataAsync.
        /// </summary>
        /// <param name="keyword">The number<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{List{AvData}}"/>.</returns>
        public async Task<List<AvData>> SearchDataAsync(string keyword)
        {
            var metaDatas = await GetMetaDataByKeyWords(keyword);
            var datas = await Task.WhenAll(metaDatas.Select(ResolveFromMetaData));
            return datas.Where((x) => x != null).ToList();
        }

        /// <summary>
        /// The GetKey.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetKey()
        {
            return "JAVDB";
        }

        /// <summary>
        /// The getRootAsync.
        /// </summary>
        /// <param name="number">The number<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{AvData}"/>.</returns>
        public async Task<AvData> GetByHtmlAsync(string number)
        {
            var metaDatas = await GetMetaDataByKeyWords(number);
            var metaData = metaDatas.FirstOrDefault(e => e.Number.Equals(number, StringComparison.OrdinalIgnoreCase));
            return await ResolveFromMetaData(metaData);
        }

        public async Task<AvData> ResolveFromMetaData(AvMetaData metaData)
        {
            if(metaData == null)
            {
                return null;
            }
            var data  = await cacheProvider.GetDataAsync(metaData.Number);
            if (data != null)
            {
                return data;
            }
            if (metaData == null || string.IsNullOrEmpty(metaData.WebSiteUrl))
            {
                return null;
            }

            var detailContent = await this.htmlContentReader.LoadFromUrlAsync(metaData.WebSiteUrl);
            if (detailContent == null)
            {
                return null;
            }
            var document = await context.OpenAsync(req => req.Content(detailContent));

            data =  ResolveContent(document, metaData);
            if (data != null)
            {
                await cacheProvider.StoreDataAsync(data);
            }
            return data;
        } 

        /// <summary>
        /// The ResolveContent.
        /// </summary>
        /// <param name="document">The document<see cref="IDocument"/>.</param>
        /// <param name="metaData">The metaData<see cref="AvMetaData"/>.</param>
        /// <returns>The <see cref="AvData"/>.</returns>
        private AvData ResolveContent(IDocument document, AvMetaData metaData)
        {
            var time = document.QuerySelector("body > section > div > div.movie-info-panel > div > div:nth-child(2) > nav > div:nth-child(2) > span")?.Text();
            var outline = document.QuerySelector("body > section > div > h2 > strong")?.Text();
            var duration = document.QuerySelector("body > section > div > div.movie-info-panel > div > div:nth-child(2) > nav > div:nth-child(3) > span")?.Text();
            var director = document.QuerySelector("body > section > div > div.movie-info-panel > div > div:nth-child(2) > nav > div:nth-child(4) > span")?.Text();
            var studio = document.QuerySelector("body > section > div > div.movie-info-panel > div > div:nth-child(2) > nav > div:nth-child(5) > span")?.Text();
            var release = document.QuerySelector("body > section > div > div.movie-info-panel > div > div:nth-child(2) > nav > div:nth-child(6) > span")?.Text();
            var category = document.QuerySelector("body > section > div > div.movie-info-panel > div > div:nth-child(2) > nav > div:nth-child(7) > span")?.Text();
            var actor = document.QuerySelector("body > section > div > div.movie-info-panel > div > div:nth-child(2) > nav > div:nth-child(8) > span")?.Text();
            var coverUrl = document.QuerySelector("body > section > div > div.movie-info-panel > div > div.column.column-video-cover > a > img") ?.GetAttribute("src");
            var previewUrl = document.QuerySelector("#preview-video > source")?.GetAttribute("src");
            var images = document.QuerySelectorAll(".tile-item").Select(e => e.GetAttribute("href")).ToList();
            var magnets = document.QuerySelectorAll(".magnet-name > a").Select(e => e.GetAttribute("href")).ToList();
            AvData data = new AvData
            {
                Title = metaData.Title,
                Number = metaData.Number,
                WebSiteUrl = metaData.WebSiteUrl,
                ThumbUrl = metaData.ThumbUrl,
                Time = time?.Trim(),
                Year = time?.Substring(0,4),
                Release = release,
                Studio = studio,
                MainCover = coverUrl,
                Actors = actor.Split(",".ToCharArray()).ToList(),
                Directors = director.Split(",".ToCharArray()).ToList(),
                PreviewVideo = previewUrl,
                Tags = category.Split(",".ToCharArray()).ToList(),
                Magnets = magnets,
                Source = this.GetKey(),
                Outline = outline


            };

            return data;
        }

        /// <summary>
        /// The getDetailPageUrl.
        /// </summary>
        /// <param name="number">The number<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        private async Task<List<AvMetaData>> GetMetaDataByKeyWords(string keyword)
        {
            var htmlContent = await this.htmlContentReader.LoadFromUrlAsync($"{this.baseUrl}/search?q={keyword}&f=all");
            if (string.IsNullOrEmpty(htmlContent))
            {
                return null;
            }

            var document = await context.OpenAsync(req => req.Content(htmlContent));
            var elements = document.QuerySelectorAll("#videos div div a");
            var eles = elements.Select(GetVideoInfo).ToList();
            return eles;
        }

        /// <summary>
        /// The GetVideoInfo.
        /// </summary>
        /// <param name="element">The element<see cref="IElement"/>.</param>
        /// <returns>The <see cref="(string href, string number,string title)"/>.</returns>
        private AvMetaData GetVideoInfo(IElement element)
        {
            var href = element.GetAttribute("href");
            var number = element.QuerySelector(".uid")?.Text();
            var title = element.QuerySelector(".video-title")?.Text();
            var thumbUrl = element.QuerySelector("img")?.Attributes["data-src"]?.Value;
            if(string.IsNullOrEmpty(href) || string.IsNullOrEmpty(number))
            {
                return null;
            }
            return new AvMetaData
            {
                Number = number,
                Title = title,
                WebSiteUrl = $"{ this.baseUrl}{href}?locale=zh",
                ThumbUrl = thumbUrl
            };
        }
    }
}
