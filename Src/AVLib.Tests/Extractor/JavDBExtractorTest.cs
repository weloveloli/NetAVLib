// -----------------------------------------------------------------------
// <copyright file="JavDBExtractorTest.cs" company="Weloveloli">
//     Copyright (c) Weloveloli.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Weloveloli.AVLib.Tests.Extractor
{
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Weloveloli.AVLib.Extractor;
    using Weloveloli.AVLib.Services;

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
            mock.Setup(x => x.LoadFromUrlAsync(It.Is<string>(x => x.Equals("https://javdb4.com/v/8VvnW?locale=zh")))).ReturnsAsync(detailContent);

            this.htmlContentReader = mock.Object;
            this.configuration = new Configuration();
            this.cacheProvider = new DictCacheProvider();
        }

        /// <summary>
        /// The MyTestMethod.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [TestMethod]
        public async Task TestGetData()
        {
            var extractor = new JavDBExtractor(htmlContentReader, cacheProvider, configuration);
            var avData = await extractor.GetDataAsync("MUM-120");
            Assert.IsNotNull(avData);
        }
    }
}
