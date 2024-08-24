using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace TelegramBotFramework.Commands.Storages.Cached {
    internal class DistributeCachedStatesStorage : ICommandsDataStorage {
        private readonly IDistributedCache cache;
        private readonly DistributedCacheEntryOptions cacheEntryOptions;

        public DistributeCachedStatesStorage(IDistributedCache cache, CachingOptions options) {
            this.cache = cache;
            cacheEntryOptions = new DistributedCacheEntryOptions {
                SlidingExpiration = options.SlidingCachingTime,
                AbsoluteExpirationRelativeToNow = options.AbsoluteCachingTime
            };
        }

        public async Task<TData?> GetData<TData>(string key) where TData : class {
            var json = await cache.GetStringAsync(key);
            if (json == null) return null;

            var data = JsonSerializer.Deserialize<TData>(json);
            return data;
        }

        public Task UpdateData<TData>(string key, TData data) where TData : class {
            var json = JsonSerializer.Serialize(data);

            return cache.SetStringAsync(key, json, cacheEntryOptions);
        }

        public Task DeleteData(string key) => cache.RemoveAsync(key);
    }
}