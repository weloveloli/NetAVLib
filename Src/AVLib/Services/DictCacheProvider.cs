﻿namespace AVCli.AVLib.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="DictCacheProvider" />.
    /// </summary>
    public class DictCacheProvider : ICacheProvider
    {
        /// <summary>
        /// Defines the dict.
        /// </summary>
        private readonly Dictionary<string, string> dict;

        /// <summary>
        /// Defines the dictObj.
        /// </summary>
        private readonly Dictionary<string, AvData> dictObj;

        /// <summary>
        /// Initializes a new instance of the <see cref="DictCacheProvider"/> class.
        /// </summary>
        public DictCacheProvider()
        {
            this.dict = new Dictionary<string, string>();
            this.dictObj = new Dictionary<string, AvData>();
        }

        /// <summary>
        /// The GetContentFromCacheAsync.
        /// </summary>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        public Task<string> GetContentFromCacheAsync(string key)
        {
            return Task.Run(() => this.dict[key]);
        }

        /// <summary>
        /// The GetDataAsync.
        /// </summary>
        /// <param name="number">The number<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{AvData}"/>.</returns>
        public Task<AvData> GetDataAsync(string number)
        {
            return Task.Run(() => this.dictObj[number]);
        }

        /// <summary>
        /// The StoreDataAsync.
        /// </summary>
        /// <param name="data">The data<see cref="AvData"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public Task<bool> StoreDataAsync(AvData data)
        {
            return Task.Run(() =>
            {
                this.dictObj[data.Number] = data;
                return true;
            });
        }

        /// <summary>
        /// The WriteCacheAsync.
        /// </summary>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <param name="content">The content<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public Task<bool> WriteCacheAsync(string key, string content)
        {
            return Task.Run(() =>
            {
                this.dict[key] = content;
                return true;
            });
        }
    }
}
