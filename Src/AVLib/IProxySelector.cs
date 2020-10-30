// IProxySelector.cs 2020

namespace AVCli.AVLib
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="IProxySelector" />.
    /// </summary>
    public interface IProxySelector
    {
        /// <summary>
        /// The GetProxyName.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetProxyName(string url);

        /// <summary>
        /// Get All of the proxy.
        /// </summary>
        /// <returns>The <see cref="List{string}"/>.</returns>
        public List<string> GetAll();
    }
}
