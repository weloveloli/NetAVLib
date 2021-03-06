// -----------------------------------------------------------------------
// <copyright file="RoundRobinProxySelectorTests.cs" company="Weloveloli">
//     Copyright (c) Weloveloli.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Weloveloli.AVLib.Services
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Weloveloli.AVLib;

    /// <summary>
    /// Defines the <see cref="RoundRobinProxySelectorTests" />.
    /// </summary>
    [TestClass]
    public class RoundRobinProxySelectorTests
    {
        /// <summary>
        /// Defines the mockRepository.
        /// </summary>
        private MockRepository mockRepository;

        /// <summary>
        /// Defines the mockConfiguration.
        /// </summary>
        private Configuration mockConfiguration;

        /// <summary>
        /// The TestInitialize.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockConfiguration = new Configuration
            {
                ProxyList = new List<string>
            {
                "https://0.0.0.0:8080",
                "https://0.0.0.0:9000"
            }
            };
        }

        /// <summary>
        /// The CreateRoundRobinProxySelector.
        /// </summary>
        /// <returns>The <see cref="RoundRobinProxySelector"/>.</returns>
        private RoundRobinProxySelector CreateRoundRobinProxySelector()
        {
            return new RoundRobinProxySelector(this.mockConfiguration);
        }

        /// <summary>
        /// The GetAll_StateUnderTest_ExpectedBehavior.
        /// </summary>
        [TestMethod]
        public void GetAll_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var roundRobinProxySelector = this.CreateRoundRobinProxySelector();

            // Act
            var result = roundRobinProxySelector.GetAll();
            Assert.AreEqual(2, result.Count);

            this.mockRepository.VerifyAll();
        }

        /// <summary>
        /// The GetProxyName_StateUnderTest_ExpectedBehavior.
        /// </summary>
        [TestMethod]
        public void GetProxyName_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var roundRobinProxySelector = this.CreateRoundRobinProxySelector();
            string url = null;

            // Act
            var result1 = roundRobinProxySelector.GetProxyName(url);
            var result2 = roundRobinProxySelector.GetProxyName(url);
            var result3 = roundRobinProxySelector.GetProxyName(url);

            // Assert
            Assert.AreNotEqual(result1, result2);
            Assert.AreEqual(result1, result3);
            Assert.AreNotEqual(result2, result3);


            this.mockRepository.VerifyAll();
        }

        /// <summary>
        /// The Test_No_Proxy.
        /// </summary>
        [TestMethod]
        public void Test_No_Proxy()
        {
            var conf = new Configuration();
            var selector = new RoundRobinProxySelector(conf);
            Assert.AreEqual(Configuration.NoProxy, selector.GetProxyName(null));
        }

        /// <summary>
        /// The Test_One_Proxy.
        /// </summary>
        [TestMethod]
        public void Test_One_Proxy()
        {
            var conf = new Configuration
            {
                ProxyList = new List<string>
                {
                    "sock5://172.168.12.10:80"
                }
            };
            var selector = new RoundRobinProxySelector(conf);
            Assert.AreEqual(1, selector.GetAll().Count);
        }
    }
}
