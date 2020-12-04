// -----------------------------------------------------------------------
// <copyright file="DictCacheProvider.cs" company="Weloveloli">
//     Copyright (c) Weloveloli.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Weloveloli.AVLib.Services
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
            if (key == null)
            {
                return Task.FromResult<string>(null); ;
            }
            return Task.Run(() => this.dict.ContainsKey(key) ? this.dict[key] : null);
        }

        /// <summary>
        /// The GetDataAsync.
        /// </summary>
        /// <param name="number">The number<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{AvData}"/>.</returns>
        public Task<AvData> GetDataAsync(string number)
        {
            if (number == null)
            {
                return Task.FromResult<AvData>(null);
            }
            return Task.Run(() => this.dictObj.ContainsKey(number) ? this.dictObj[number] : null);
        }

        /// <summary>
        /// The StoreDataAsync.
        /// </summary>
        /// <param name="data">The data<see cref="AvData"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public Task<bool> StoreDataAsync(AvData data)
        {
            if (data == null)
            {
                return Task.FromResult(false);
            }
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
            if (content == null || key == null)
            {
                return Task.FromResult(false);
            }
            return Task.Run(() =>
            {
                this.dict[key] = content;
                return true;
            });
        }
    }
}
