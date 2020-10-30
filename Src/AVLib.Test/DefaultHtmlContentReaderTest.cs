// UnitTest1.cs 2020

namespace AVLib.Test
{
    using AVCli.AVLib;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Moq.Protected;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="DefaultHtmlContentReaderTest" />.
    /// </summary>
    [TestClass]
    public class DefaultHtmlContentReaderTest
    {
        /// <summary>
        /// Defines the httpFactory.
        /// </summary>
        private Mock<IHttpClientFactory> httpFactory;

        /// <summary>
        /// Defines the cacheProvider.
        /// </summary>
        private Mock<ICacheProvider> cacheProvider;

        /// <summary>
        /// Defines the proxySelector.
        /// </summary>
        private Mock<IProxySelector> proxySelector;

        /// <summary>
        /// The Init.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            // Mock the handler
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(e => e.Method == HttpMethod.Get), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("success")
                });
            // create the mock client factory mock
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            // setup the method call
            httpClientFactoryMock.Setup(x => x.CreateClient(Configuration.NoProxy))
                                 .Returns(new HttpClient(handlerMock.Object));

            this.httpFactory = httpClientFactoryMock;
            this.cacheProvider = new Mock<ICacheProvider>();
            this.cacheProvider.Setup(x => x.StoreDataAsync(It.IsAny<string>(), It.IsAny<AvData>())).ReturnsAsync(true);
            this.cacheProvider.Setup(x => x.WriteCacheAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            this.cacheProvider.Setup(x => x.GetContentFromCacheAsync(It.IsAny<string>())).ReturnsAsync(string.Empty);
            this.cacheProvider.Setup(x => x.GetDataAsync(It.IsAny<string>())).ReturnsAsync((AvData)null);

            this.proxySelector = new Mock<IProxySelector>();
            this.proxySelector.Setup(x => x.GetAll()).Returns(new List<string>());
        }

        /// <summary>
        /// The TestLoadFromUrlAsync.
        /// </summary>
        [TestMethod]
        public void TestLoadFromUrlAsync()
        {
        }
    }
}