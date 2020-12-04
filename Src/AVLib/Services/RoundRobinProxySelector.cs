// -----------------------------------------------------------------------
// <copyright file="RoundRobinProxySelector.cs" company="Weloveloli">
//     Copyright (c) Weloveloli.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Weloveloli.AVLib.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Weloveloli.AVLib;
    using Weloveloli.AVLib.Extensions;

    /// <summary>
    /// Defines the <see cref="RoundRobinProxySelector" />.
    /// </summary>
    public class RoundRobinProxySelector : IProxySelector
    {
        /// <summary>
        /// Defines the conf.
        /// </summary>
        private readonly Configuration conf;

        /// <summary>
        /// Defines the _increasement.
        /// </summary>
        private int _increasement;

        /// <summary>
        /// Defines the proxies.
        /// </summary>
        private readonly List<string> proxies;

        /// <summary>
        /// Defines the proxyName.
        /// </summary>
        private readonly List<string> proxyName;

        /// <summary>
        /// Defines the keyValues.
        /// </summary>
        private readonly Dictionary<string, string> keyValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoundRobinProxySelector"/> class.
        /// </summary>
        /// <param name="conf">The conf<see cref="AVLibConf"/>.</param>
        public RoundRobinProxySelector(Configuration conf)
        {
            this.conf = conf;
            this._increasement = 0;
            if (conf.UseProxy)
            {
                this.proxies = new List<string>(new HashSet<string>(conf.ProxyList));
                this.proxyName = this.proxies.Select(x => x.SHA256()).ToList();
                this.keyValues = this.proxies.Select(x => new KeyValuePair<string, string>(x.SHA256(), x)).ToDictionary(x => x.Key, x => x.Value);

            }
            else
            {
                this.proxies = null;
                this.proxyName = null;
                this.keyValues = null;
            }
        }

        /// <summary>
        /// The GetAll.
        /// </summary>
        /// <returns>The <see cref="List{string}"/>.</returns>
        public Dictionary<string, string> GetAll() => keyValues;

        /// <summary>
        /// The GetProxyName.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetProxyName(string url)
        {
            if (conf.UseProxy)
            {
                if (this.proxyName.Count == 1)
                {
                    return proxyName[0];
                }
                else
                {
                    var proxy = proxyName[Interlocked.Increment(ref _increasement) % this.proxyName.Count];
                    return proxy;
                }
            }
            else
            {
                return Configuration.NoProxy;
            }
        }
    }
}
