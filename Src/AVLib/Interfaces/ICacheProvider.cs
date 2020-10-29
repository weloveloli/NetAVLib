// ICacheProvider.cs 2020

namespace AVCli.AVLib.Interfaces
{
    /// <summary>
    /// Defines the <see cref="ICacheProvider" />.
    /// </summary>
    interface ICacheProvider
    {
        /// <summary>
        /// Get content from cache.
        /// </summary>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <returns>content from cache.</returns>
        public string GetContentFromCache(string key);

        /// <summary>
        /// Write content into cache.
        /// </summary>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <param name="content">content value.</param>
        /// <returns>if write to cache is success.</returns>
        bool WriteCache(string key, string content);
    }
}
