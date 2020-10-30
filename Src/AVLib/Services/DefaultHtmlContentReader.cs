// DefaultHtmlContentReader.cs 2020

namespace AVCli.AVLib.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="DefaultHtmlContentReader" />.
    /// </summary>
    public class DefaultHtmlContentReader : IHtmlContentReader
    {
        /// <summary>
        /// Defines the cacheProvider.
        /// </summary>
        private readonly ICacheProvider cacheProvider;

        /// <summary>
        /// Defines the proxySelector.
        /// </summary>
        private readonly IProxySelector proxySelector;

        /// <summary>
        /// Defines the aVLibConf.
        /// </summary>
        private readonly Configuration conf;

        /// <summary>
        /// Defines the clientFactory.
        /// </summary>
        private readonly IHttpClientFactory clientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultHtmlContentReader"/> class.
        /// </summary>
        /// <param name="clientFactory">The clientFactory<see cref="IHttpClientFactory"/>.</param>
        /// <param name="cacheProvider">The cacheProvider<see cref="ICacheProvider"/>.</param>
        /// <param name="proxySelector">The proxySelector<see cref="IProxySelector"/>.</param>
        /// <param name="conf">The aVLibConf<see cref="Configuration"/>.</param>
        public DefaultHtmlContentReader(IHttpClientFactory clientFactory, ICacheProvider cacheProvider, IProxySelector proxySelector, Configuration conf)
        {
            this.cacheProvider = cacheProvider;
            this.proxySelector = proxySelector;
            this.conf = conf;
            this.clientFactory = clientFactory;
        }

        /// <summary>
        /// The LoadFromUrlAsync.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        public async Task<string> LoadFromUrlAsync(string url)
        {
            var contentFromCache = await cacheProvider.GetContentFromCacheAsync(url);
            if (!string.IsNullOrEmpty(contentFromCache))
            {
                return contentFromCache;
            }
            var proxyName = proxySelector.GetProxyName(url);
            var httpClient = clientFactory.CreateClient(proxyName);
            httpClient.DefaultRequestHeaders.Add("UserAgent", conf.UserAgent);
            HttpResponseMessage message = await httpClient.GetAsync(url);
            message.EnsureSuccessStatusCode();
            var responseBody = await message.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}
