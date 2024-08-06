using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using TelegramBotFramework.Storages.Contracts;
using TelegramBotFramework.Storages.Options;

namespace TelegramBotFramework.Storages.DistributeCached {
    internal class CommandStepsDataDistributeCached(
        IDistributedCache cache,
        CachingOptions options)
        : ICommandStepsDataStorage {
        private readonly TimeSpan cacheTime = TimeSpan.FromMinutes(options.CacheTimeInMinutes);

        public async Task<TData?> GetData<TData>(long chatId, long userId) where TData : class {
            var key = GetKey(chatId, userId);

            var json = await cache.GetStringAsync(key);
            if (json == null) return null;

            var data = JsonSerializer.Deserialize<TData>(json);
            return data;
        }

        public Task SetData<TData>(long chatId, long userId, TData data) where TData : class {
            var key = GetKey(chatId, userId);
            var json = JsonSerializer.Serialize(data);

            var options = new DistributedCacheEntryOptions { SlidingExpiration = cacheTime };
            return cache.SetStringAsync(key, json, options);
        }

        private static string GetKey(long chatId, long userId) => $"{chatId}-{userId}";
    }
}