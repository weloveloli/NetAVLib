// JavDBExtractorTest.cs 2020

namespace AVCli.AVLib.Tests.Extractor
{
    using AVCli.AVLib.Extractor;
    using AVCli.AVLib.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="JavDBExtractorTest" />.
    /// </summary>
    [TestClass]
    public class JavDBExtractorTest
    {
        /// <summary>
        /// Defines the htmlContentReader.
        /// </summary>
        private IHtmlContentReader htmlContentReader;

        /// <summary>
        /// Defines the configuration.
        /// </summary>
        private Configuration configuration;

        /// <summary>
        /// Defines the cacheProvider.
        /// </summary>
        private DictCacheProvider cacheProvider;

        /// <summary>
        /// The Init.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            Mock<IHtmlContentReader> mock = new Mock<IHtmlContentReader>();
            var searchContent = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Test\\JAVDB\\SearchIndex.html"));
            var detailContent = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Test\\JAVDB\\detail.html"));
            mock.Setup(x => x.LoadFromUrlAsync(It.Is<string>(x => x.Equals("https://javdb4.com/search?q=MUM-120&f=all")))).ReturnsAsync(searchContent);
            mock.Setup(x => x.LoadFromUrlAsync(It.Is<string>(x => x.Equals("https://javdb4.com/v/qMR56?locale=zh")))).ReturnsAsync(searchContent);

            this.htmlContentReader = mock.Object;
            this.configuration = new Configuration();
            this.cacheProvider = new DictCacheProvider();
        }

        /// <summary>
        /// The MyTestMethod.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [TestMethod]
        public async Task MyTestMethod()
        {
            var extractor = new JavDBExtractor(htmlContentReader, cacheProvider, configuration);
            var url = await extractor.GetDataAsync("MUM-120");
        }
    }
}
