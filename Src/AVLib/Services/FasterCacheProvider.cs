namespace AVCli.AVLib.Services
{
    using AVCli.AVLib.Configuration;
    using AVCli.AVLib.Interfaces;
    using FASTER.core;
    using System;
    using System.IO;

    /// <summary>
    /// Defines the <see cref="FasterCacheProvider" />.
    /// </summary>
    internal class FasterCacheProvider : ICacheProvider
    {
        /// <summary>
        /// Defines the conf.
        /// </summary>
        private readonly AVLibConf conf;
        private readonly FasterKV<string, string> kv;



        /// <summary>
        /// Initializes a new instance of the <see cref="FasterCacheProvider"/> class.
        /// </summary>
        /// <param name="conf">The conf<see cref="AVLibConf"/>.</param>
        public FasterCacheProvider(AVLibConf conf)
        {
            this.conf = conf;
            var log = Devices.CreateLogDevice(Path.Combine(conf.BasePath,"faster.log"));
            this.kv = new FasterKV<string, string>(1L << 20, new LogSettings { LogDevice = log });
        }

        /// <summary>
        /// The GetContentFromCache.
        /// </summary>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetContentFromCache(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The WriteCache.
        /// </summary>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <param name="content">The content<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool WriteCache(string key, string content)
        {
            throw new NotImplementedException();
        }


    }
}
