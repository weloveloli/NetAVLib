// -----------------------------------------------------------------------
// <copyright file="LiteDbCahceProviderTest.cs" company="Weloveloli">
//     Copyright (c) Weloveloli.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Weloveloli.AVLib
{
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Weloveloli.AVLib.Services;

    /// <summary>
    /// Defines the <see cref="DictCacheProviderTest" />.
    /// </summary>
    [TestClass]
    public class DictCacheProviderTest
    {
        /// <summary>
        /// Defines the provider.
        /// </summary>
        private DictCacheProvider provider;

        /// <summary>
        /// The Init.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            this.provider = new DictCacheProvider();
        }

        /// <summary>
        /// The MyTestMethod.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [TestMethod]
        public async Task TestCache()
        {
            bool store = await this.provider.StoreDataAsync(new AvData()
            {
                Title = "AV-Test",
                Number = "AV-Test"
            });
            Assert.IsTrue(store);
            AvData data = await provider.GetDataAsync("AV-Test");
            Assert.IsNotNull(data);
            Assert.AreEqual("AV-Test", data.Title);
        }

        /// <summary>
        /// The TestGetContent.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [TestMethod]
        public async Task TestGetContent()
        {
            bool store = await provider.WriteCacheAsync("abc", "efg");

            Assert.IsTrue(store);
            string content = await provider.GetContentFromCacheAsync("abc");
            Assert.IsNotNull(content);
            Assert.AreEqual("efg", content);
        }

        /// <summary>
        /// The TestSaveNull.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [TestMethod]
        public async Task TestSaveNull()
        {
            var store = await provider.WriteCacheAsync("abc", null);
            Assert.IsFalse(store);
        }

        /// <summary>
        /// The TestSaveNull2.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [TestMethod]
        public async Task TestSaveNull2()
        {
            var store = await provider.WriteCacheAsync(null, "abc");
            Assert.IsFalse(store);
        }

        /// <summary>
        /// The TestGetNullContent.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [TestMethod]
        public async Task TestGetNullContent()
        {
            AvData data = await provider.GetDataAsync(null);
            Assert.IsNull(data);
        }

        /// <summary>
        /// The TestGetNullContent2.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [TestMethod]
        public async Task TestGetNullContent2()
        {
            AvData data = await provider.GetDataAsync(null);
            Assert.IsNull(data);
        }
    }
}
