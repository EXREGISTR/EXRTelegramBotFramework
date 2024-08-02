using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TelegramBotFramework.Storages.Contracts;
using TelegramBotFramework.Storages.Options;

namespace TelegramBotFramework.Storages.MemoryCached {
    internal class CommandStepsDataMemoryCached(
        IMemoryCache cache,
        IOptions<CachingOptions> options) 
        : ICommandStepsDataStorage {
        private readonly TimeSpan cacheMinutes = TimeSpan.FromMinutes(options.Value.CacheTimeInMinutes);

        public Task<TData?> GetData<TData>(long chatId, long userId) where TData : class {
            var key = GetKey(chatId, userId);

            var data = cache.Get<TData>(key);

            return Task.FromResult(data);
        }

        public Task SetData<TData>(long chatId, long userId, TData data) where TData : class {
            var key = GetKey(chatId, userId);
            cache
                .CreateEntry(key)
                .SetValue(data)
                .SetSlidingExpiration(cacheMinutes); ;

            return Task.CompletedTask;
        }

        private static string GetKey(long chatId, long userId) => $"{chatId}-{userId}";
    }
}
