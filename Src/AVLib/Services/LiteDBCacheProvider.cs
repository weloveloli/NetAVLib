namespace AVCli.AVLib.Services
{
    using AVCli.AVLib.Extensions;
    using LiteDB;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="LiteDBCacheProvider" />.
    /// </summary>
    internal class LiteDBCacheProvider : ICacheProvider, IDisposable
    {
        /// <summary>
        /// Defines the conf.
        /// </summary>
        private readonly LiteDatabase db;
        private bool disposedValue;
        private readonly ILiteStorage<string> fs;
        private readonly ILiteCollection<AvData> avCol;


        /// <summary>
        /// Initializes a new instance of the <see cref="LiteDBCacheProvider"/> class.
        /// </summary>
        /// <param name="conf">The conf<see cref="AVLibConf"/>.</param>
        public LiteDBCacheProvider(Configuration conf)
        {
            this.db = new LiteDatabase(Path.Combine(conf.BasePath, "cache.db"));
            this.disposedValue = false;
            this.fs = this.db.GetStorage<string>("htmlContent", "htmlContentChunk");
            this.avCol = this.db.GetCollection<AvData>("avdata");
            this.avCol.EnsureIndex(x => x.Number);
        }

        /// <summary>
        /// The GetContentFromCache.
        /// </summary>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public Task<string> GetContentFromCacheAsync(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;
            var id = key.SHA256();
            var liteFileInfo = this.fs.FindById(id);
            if (liteFileInfo == null) return null;
            using var stream = new MemoryStream();
            liteFileInfo.CopyTo(stream);
            stream.Position = 0;
            StreamReader streamReader = new StreamReader(stream);
            return streamReader.ReadToEndAsync();
        }

        /// <summary>
        /// The WriteCache.
        /// </summary>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <param name="content">The content<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public async Task<bool> WriteCacheAsync(string key, string content)
        {
            if (string.IsNullOrEmpty(key)||string.IsNullOrEmpty(content))
            {
                return false;
            }
            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream);
            await writer.WriteAsync(content);
            this.fs.Upload(key.SHA256(), key, stream);
            return true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.db.Dispose();
                }
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~FasterCacheProvider()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async Task<AvData> GetDataAsync(string number)
        {
            return await Task.Run(()=>this.avCol.FindOne(x => x.Number == number));
        }

        public async Task<bool> StoreDataAsync(string number, AvData data)
        {
            return await Task.Run(() =>
            {
                if (string.IsNullOrEmpty(number) || data == null)
                {
                    return false;
                }
                avCol.Insert(data);
                return true;
            });
        }
    }
}
