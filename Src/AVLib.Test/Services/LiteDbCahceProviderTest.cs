namespace AVCli.AVLib
{
    using AVCli.AVLib.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="DictCacheProviderTest" />.
    /// </summary>
    [TestClass]
    public class DictCacheProviderTest
    {
        private DictCacheProvider provider;

        [TestInitialize]
        public void Init()
        {
            this.provider = new DictCacheProvider();
        }

        /// <summary>
        /// The MyTestMethod.
        /// </summary>
        [TestMethod]
        public async Task TestCache()
        {
            bool store = await this.provider.StoreDataAsync(new AvData()
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
    