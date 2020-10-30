// RoundRobinProxySelector.cs 2020

namespace AVCli.AVLib.Services
{
    using AVCli.AVLib.Configuration;
    using AVCli.AVLib.Interfaces;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// Defines the <see cref="RoundRobinProxySelector" />.
    /// </summary>
    public class RoundRobinProxySelector : IProxySelector
    {
        /// <summary>
        /// Defines the conf.
        /// </summary>
        private readonly AVLibConf conf;

        private int _increasement;

        private readonly List<string> proxies;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoundRobinProxySelector"/> class.
        /// </summary>
        /// <param name="conf">The conf<see cref="AVLibConf"/>.</param>
        public RoundRobinProxySelector(AVLibConf conf)
        {
            this.conf = conf;
            this._increasement = 0;
            this.proxies = conf.ProxyList;
        }

        /// <summary>
        /// The GetAll.
        /// </summary>
        /// <returns>The <see cref="List{string}"/>.</returns>
        public List<string> GetAll()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// The GetProxyName.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetProxyName(string url)
        {
            if (conf.UseProxy)
            {
                if (this.proxies.Count == 1)
                {
                    return proxies[0];
                }
                else
                {
                    var proxy = proxies[Interlocked.Increment(ref _increasement) % this.proxies.Count];
                    return proxy;
                }

            }
            else
            {
                return AVLibConf.NoProxy;
            }
        }
    }
}
