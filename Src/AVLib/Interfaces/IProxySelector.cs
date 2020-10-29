// IProxySelector.cs 2020

namespace AVCli.AVLib.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="IProxySelector" />.
    /// </summary>
    interface IProxySelector
    {
        /// <summary>
        /// The GetProxyName.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        string GetProxyName(string url);

        /// <summary>
        /// Get All of the proxy.
        /// </summary>
        /// <returns>The <see cref="List{string}"/>.</returns>
        List<string> GetAll();
    }
}
