// JavDBExtractor.cs 2020

namespace AVCli.AVLib.Extractor
{
    using AngleSharp;
    using AngleSharp.Dom;
    using System;
    using System.Linq;
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
            var data = await this.GetDataAsync(number);
            if (data != null)
            {
                return data;
            }
            data = await this.getRootAsync(number);
            await this.cacheProvider.StoreDataAsync(data);
            return data;
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
        public async Task<AvData> getRootAsync(string number)
        {
            var detailUrl = await getDetailPageUrl(number);
            if (detailUrl == null)
            {
                return null;
            }
            return null;
        }

        /// <summary>
        /// The getDetailPageUrl.
        /// </summary>
        /// <param name="number">The number<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        public async Task<string> getDetailPageUrl(string number)
        {
            var htmlContent = await this.htmlContentReader.LoadFromUrlAsync($"{this.baseUrl}/search?q={number}&f=all");
            if (string.IsNullOrEmpty(htmlContent))
            {
                return null;
            }

            var document = await context.OpenAsync(req => req.Content(htmlContent));

            var elements = document.QuerySelectorAll("#videos div div a");
            var ele = elements.Select(GetVideoInfo).FirstOrDefault(e => e.number.Equals(number, StringComparison.OrdinalIgnoreCase));
            if (string.IsNullOrEmpty(ele.href))
            {
                return null;
            }
            return $"{ this.baseUrl}{ele.href}";
        }

        /// <summary>
        /// The GetVideoInfo.
        /// </summary>
        /// <param name="element">The element<see cref="IElement"/>.</param>
        /// <returns>The <see cref="(string href, string number,string title)"/>.</returns>
        internal static (string href, string number, string title) GetVideoInfo(IElement element)
        {
            var href = element.GetAttribute("href");
            var number = element.QuerySelector(".uid").Text();
            var title = element.QuerySelector(".video-title").Text();
            return (href, number, title);
        }
    }
}
