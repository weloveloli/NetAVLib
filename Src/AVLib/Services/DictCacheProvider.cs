using AngleSharp.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AVCli.AVLib.Services
{
    public class DictCacheProvider : ICacheProvider
    {
        private readonly Dictionary<string, string> dict;
        private readonly Dictionary<string, AvData> dictObj;

        public DictCacheProvider()
        {
            this.dict = new Dictionary<string, string>();
            this.dictObj = new Dictionary<string, AvData>();
        }

        public Task<string> GetContentFromCacheAsync(string key)
        {
            return Task.Run(() => this.dict[key]);
        }

        public Task<AvData> GetDataAsync(string number)
        {
            return Task.Run(() => this.dictObj[number]);
        }

        public Task<bool> StoreDataAsync(string number, AvData data)
        {
            return Task.Run(() =>
            {
                this.dictObj[number] = data;
                return true;
            });
        }

        public Task<bool> WriteCacheAsync(string key, string content)
        {
            return Task.Run(() =>
            {
                this.dict[key] = content;
                return true;
            });
        }
    }
}
