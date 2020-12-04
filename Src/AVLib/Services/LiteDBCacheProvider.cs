// -----------------------------------------------------------------------
// <copyright file="LiteDBCacheProvider.cs" company="Weloveloli">
//     Copyright (c) Weloveloli.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Weloveloli.AVLib.Services
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using LiteDB;
    using Weloveloli.AVLib.Extensions;

    /// <summary>
    /// Defines the <see cref="LiteDBCacheProvider" />.
    /// </summary>
    public class LiteDBCacheProvider : ICacheProvider, IDisposable
    {
        /// <summary>
        /// Defines the db.
        /// </summary>
        private readonly LiteDatabase db;

        /// <summary>
        /// Defines the disposedValue.
        /// </summary>
        private bool disposedValue;

        /// <summary>
        /// Defines the fs.
        /// </summary>
        private readonly ILiteStorage<string> fs;

        /// <summary>
        /// Defines the avCol.
        /// </summary>
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
        public async Task<string> GetContentFromCacheAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            var id = key.SHA256();
            var liteFileInfo = this.fs.FindById(id);
            if (liteFileInfo == null)
            {
                return null;
            }

            using var stream = new MemoryStream();
            liteFileInfo.CopyTo(stream);
            stream.Position = 0;
            StreamReader streamReader = new StreamReader(stream);
            return await streamReader.ReadToEndAsync();
        }

        /// <summary>
        /// The WriteCache.
        /// </summary>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <param name="content">The content<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public async Task<bool> WriteCacheAsync(string key, string content)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(content))
            {
                return false;
            }
            using (var stream = new MemoryStream())
            {
                using var writer = new StreamWriter(stream);
                await writer.WriteAsync(content);
                await writer.FlushAsync();
                stream.Position = 0;
                this.fs.Upload(key.SHA256(), key, stream);
            }
            return true;
        }

        /// <summary>
        /// The Dispose.
        /// </summary>
        /// <param name="disposing">The disposing<see cref="bool"/>.</param>
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

        /// <summary>
        /// The Dispose.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The GetDataAsync.
        /// </summary>
        /// <param name="number">The number<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{AvData}"/>.</returns>
        public async Task<AvData> GetDataAsync(string number)
        {
            return await Task.Run(() => this.avCol.FindOne(x => x.Number == number));
        }

        /// <summary>
        /// The StoreDataAsync.
        /// </summary>
        /// <param name="data">The data<see cref="AvData"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> StoreDataAsync(AvData data)
        {
            return await Task.Run(() =>
            {
                if (data == null)
                {
                    return false;
                }
                avCol.Insert(data);
                return true;
            });
        }
    }
}
