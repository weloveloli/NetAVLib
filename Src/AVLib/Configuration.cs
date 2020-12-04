// -----------------------------------------------------------------------
// <copyright file="Configuration.cs" company="Weloveloli">
//     Copyright (c) Weloveloli.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Weloveloli.AVLib
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="Configuration" />.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Defines the UserAgentMap.
        /// </summary>
        public static readonly Dictionary<string, string> UserAgentMap = new Dictionary<string, string>
        {
            { "Chrome","Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.835.163 Safari/535.1"},
             { "Firefox","Mozilla/5.0 (Windows NT 6.1; WOW64; rv:6.0) Gecko/20100101 Firefox/6.0"},
            { "Safari","Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/534.50 (KHTML, like Gecko) Version/5.1 Safari/534.50"}
        };

        /// <summary>
        /// Defines the DefaultBaseUrlMap.
        /// </summary>
        private static readonly Dictionary<string, string> DefaultBaseUrlMap = new Dictionary<string, string>
        {
            { "JAVDB","https://javdb4.com"}
        };

        /// <summary>
        /// Defines the NoProxy.
        /// </summary>
        public const string NoProxy = "default";

        /// <summary>
        /// Defines the userAgent.
        /// </summary>
        private string userAgent;

        /// <summary>
        /// Gets or sets the UserAgent.
        /// </summary>
        public string UserAgent
        {
            get
            {
                return string.IsNullOrEmpty(userAgent) ? UserAgentMap["Chrome"] : userAgent;
            }
            set
            {
                if (UserAgentMap.ContainsKey(value))
                {
                    userAgent = UserAgentMap[value];
                }
                else
                {
                    userAgent = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the ProxyList.
        /// </summary>
        public List<string> ProxyList { get; set; }

        /// <summary>
        /// Gets a value indicating whether UseProxy.
        /// </summary>
        public bool UseProxy => this.ProxyList != null && this.ProxyList.Any();

        /// <summary>
        /// Defines the basePath.
        /// </summary>
        private string basePath;

        /// <summary>
        /// Gets or sets the BasePath.
        /// </summary>
        public string BasePath
        {
            get
            {
                return this.basePath ?? System.IO.Directory.GetCurrentDirectory();
            }
            set { this.basePath = value; }
        }

        /// <summary>
        /// The GetBaseUrl.
        /// </summary>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetBaseUrl(string key) => Environment.GetEnvironmentVariable("BASE_URL_" + key) ?? DefaultBaseUrlMap[key];
    }
}
