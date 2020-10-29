// DefaultHtmlContentReader.cs 2020

namespace AVCli.AVLib.Services
{
    using AVCli.AVLib.Configuration;
    using AVCli.AVLib.Interfaces;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="DefaultHtmlContentReader" />.
    /// </summary>
    internal class DefaultHtmlContentReader : IHtmlContentReader
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
        private readonly AVLibConf aVLibConf;

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
        /// <param name="aVLibConf">The aVLibConf<see cref="AVLibConf"/>.</param>
        public DefaultHtmlContentReader(IHttpClientFactory clientFactory, ICacheProvider cacheProvider, IProxySelector proxySelector, AVLibConf aVLibConf)
        {
            this.cacheProvider = cacheProvider;
            this.proxySelector = proxySelector;
            this.aVLibConf = aVLibConf;
            this.clientFactory = clientFactory;
        }

        /// <summary>
        /// The LoadFromUrlAsync.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        public async Task<string> LoadFromUrlAsync(string url)
        {
            var contentFromCache = cacheProvider.GetContentFromCache(url);
            if (!string.IsNullOrEmpty(contentFromCache))
            {
                return contentFromCache;
            }
            var proxyName = proxySelector.GetProxyName(url);
            var httpClient = clientFactory.CreateClient(proxyName);
            httpClient.DefaultRequestHeaders.Add("UserAgent", aVLibConf.UserAgent);
            HttpResponseMessage message = await httpClient.GetAsync(url);
            message.EnsureSuccessStatusCode();
            var responseBody = await message.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}
