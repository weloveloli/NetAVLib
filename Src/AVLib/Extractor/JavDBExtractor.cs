namespace AVCli.AVLib.Extractor
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="JavDBExtractor" />.
    /// </summary>
    public class JavDBExtractor : IExtractor
    {
        /// <summary>
        /// Defines the cacheProvider.
        /// </summary>
        private readonly ICacheProvider cacheProvider;

        /// <summary>
        /// Defines the conf.
        /// </summary>
        private readonly Configuration conf;

        /// <summary>
        /// Defines the htmlContentReader.
        /// </summary>
        private readonly IHtmlContentReader htmlContentReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="JavDBExtractor"/> class.
        /// </summary>
        /// <param name="htmlContentReader">The htmlContentReader<see cref="IHtmlContentReader"/>.</param>
        /// <param name="cacheProvider">The cacheProvider<see cref="ICacheProvider"/>.</param>
        /// <param name="configuration">The configuration<see cref="Configuration"/>.</param>
        public JavDBExtractor(IHtmlContentReader htmlContentReader, ICacheProvider cacheProvider, Configuration configuration)
        {
            this.htmlContentReader = htmlContentReader;
            this.cacheProvider = cacheProvider;
            this.conf = configuration;
        }

        /// <summary>
        /// The GetDataAsync.
        /// </summary>
        /// <param name="number">The number<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{AvData}"/>.</returns>
        public Task<AvData> GetDataAsync(string number)
        {
            throw new NotImplementedException();
        }
    }
}
