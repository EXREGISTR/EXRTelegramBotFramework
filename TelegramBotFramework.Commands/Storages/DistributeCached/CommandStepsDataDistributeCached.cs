using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using TelegramBotFramework.Commands.Storages.Contracts;
using TelegramBotFramework.Storages.Options;

namespace TelegramBotFramework.Commands.Storages.DistributeCached {
    internal class CommandStepsDataDistributeCached(
        IDistributedCache cache,
        CachingOptions options)
        : ICommandStepsDataStorage {
        private readonly DistributedCacheEntryOptions cacheEntryOptions =
            new() { SlidingExpiration = options.CachingTime, };

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

            return cache.SetStringAsync(key, json, cacheEntryOptions);
        }

        private static string GetKey(long chatId, long userId) => $"{chatId}-{userId}";

        public Task DeleteData(long chatId, long userId) => cache.RemoveAsync(GetKey(chatId, userId));
    }
}