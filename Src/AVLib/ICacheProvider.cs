// ICacheProvider.cs 2020

namespace AVCli.AVLib
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ICacheProvider" />.
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// Get content from cache.
        /// </summary>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <returns>content from cache.</returns>
        public Task<string> GetContentFromCacheAsync(string key);

        /// <summary>
        /// Write content into cache.
        /// </summary>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <param name="content">content value.</param>
        /// <returns>if write to cache is success.</returns>
        public Task<bool> WriteCacheAsync(string key, string content);

        /// <summary>
        /// The GetData.
        /// </summary>
        /// <param name="number">The number<see cref="string"/>.</param>
        /// <returns>The <see cref="AvData"/>.</returns>
        public Task<AvData> GetDataAsync(string number);

        /// <summary>
        /// The StoreData.
        /// </summary>
        /// <param name="data">The data<see cref="AvData"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public Task<bool> StoreDataAsync(AvData data);
    }
}
