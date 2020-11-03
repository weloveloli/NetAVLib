namespace AVCli.AVLib
{
    using AVCli.AVLib.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="LiteDbCahceProviderTest" />.
    /// </summary>
    [TestClass]
    public class LiteDbCahceProviderTest
    {
        private Configuration configuration;
        private string rootDir;
        private LiteDBCacheProvider provider;

        [TestInitialize]
        public void Init()
        {
            this.rootDir = Path.Combine(Path.GetTempPath(),"avlibtest");
            if (!Directory.Exists(rootDir))
            {
                Directory.CreateDirectory(rootDir);
            }
            this.configuration = new Configuration
            {
                BasePath = rootDir
            };
            this.provider = new LiteDBCacheProvider(this.configuration);
        }

        [TestCleanup]
        public void CleanUp()
        {
            provider.Dispose();
            Directory.Delete(rootDir,true);
        }
        /// <summary>
        /// The MyTestMethod.
        /// </summary>
        [TestMethod]
        public async Task TestCache()
        {
            bool store = await provider.StoreDataAsync(new AvData()
            {
                Title="AV-Test",Number = "AV-Test"
            });
            Assert.IsTrue(store);
            AvData data = await provider.GetDataAsync("AV-Test");
            Assert.IsNotNull(data);
            Assert.AreEqual("AV-Test", data.Title);
        }

        [TestMethod]
        public async Task TestGetContent()
        {
            bool store = await provider.WriteCacheAsync("abc", "efg");

            Assert.IsTrue(store);
            string content = await provider.GetContentFromCacheAsync("abc");
            Assert.IsNotNull(content);
            Assert.AreEqual("efg", content);
        }


        [TestMethod]
        public async Task TestSaveNull()
        {
            var store = await provider.WriteCacheAsync("abc", null);
            Assert.IsFalse(store);
        }

        [TestMethod]
        public async Task TestSaveNull2()
        {
            var store = await provider.WriteCacheAsync(null, "abc");
            Assert.IsFalse(store);
        }
        [TestMethod]
        public async Task TestGetNullContent()
        {
            AvData data = await provider.GetDataAsync(null);
            Assert.IsNull(data);
        }
        [TestMethod]
        public async Task TestGetNullContent2()
        {
            AvData data = await provider.GetDataAsync(null);
            Assert.IsNull(data);
        }
    }
}
    