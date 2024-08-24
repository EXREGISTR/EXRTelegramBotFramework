using Microsoft.Extensions.Caching.Memory;

namespace TelegramBotFramework.Commands.Storages.Cached {
    internal class MemoryCachedStatesStorage : ICommandsDataStorage {
        private readonly IMemoryCache cache;
        private readonly MemoryCacheEntryOptions entryOptions;

        public MemoryCachedStatesStorage(IMemoryCache cache, CachingOptions options) {
            this.cache = cache;
            entryOptions = new MemoryCacheEntryOptions {
                AbsoluteExpirationRelativeToNow = options.AbsoluteCachingTime,
                SlidingExpiration = options.SlidingCachingTime,
            };
        }

        public Task<TData?> GetData<TData>(string key) where TData : class {
            var data = cache.Get<TData>(key);
            return Task.FromResult(data);
        }

        public Task UpdateData<TData>(string key, TData data) where TData : class {
            if (cache.TryGetValue(key, out var exists)) {
                if (data.Equals(exists as TData)) {
                    return Task.CompletedTask;
                }
            }

            cache
                .CreateEntry(key)
                .SetValue(data)
                .SetOptions(entryOptions);

            return Task.CompletedTask;
        }

        public Task DeleteData(string key) {
            cache.Remove(key);
            return Task.CompletedTask;
        }
    }
}
