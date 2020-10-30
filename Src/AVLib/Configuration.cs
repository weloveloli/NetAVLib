﻿// Configuration.cs 2020

namespace AVCli.AVLib
{
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
        /// Defines the NoProxy.
        /// </summary>
        public const string NoProxy = "default";

        /// <summary>
        /// Gets or sets the UserAgent.
        /// </summary>
        public string UserAgent
        {
            get { return string.IsNullOrEmpty(UserAgent) ? UserAgentMap["Chrome"] : UserAgent; }
            set
            {
                if (UserAgentMap.ContainsKey(value))
                {
                    UserAgent = UserAgentMap[value];
                }
                else
                {
                    UserAgent = value;
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
        public bool UseProxy => this.ProxyList == null && this.ProxyList.Any();

        /// <summary>
        /// Gets or sets the BasePath.
        /// </summary>
        public string BasePath
        {
            get
            {
                return BasePath ?? System.IO.Directory.GetCurrentDirectory();
            }
            set { BasePath = value; }
        }
    }
}