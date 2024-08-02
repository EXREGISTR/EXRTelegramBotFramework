using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TelegramBotFramework.Commands;
using TelegramBotFramework.Storages.Contracts;
using TelegramBotFramework.Storages.Options;

namespace TelegramBotFramework.Storages.DistributeCached {
    internal class UserStatesDistributeCached(
        IDistributedCache cache,
        IOptions<CachingOptions> options) : IUserStatesStorage {
        private readonly TimeSpan cacheTime = TimeSpan.FromMinutes(options.Value.CacheTimeInMinutes);

        public async Task<CommandStepIdentity?> GetActiveStep(long chatId, long userId) {
            string key = GetKey(chatId, userId);

            var json = await cache.GetStringAsync(key);
            if (json == null) return null;

            var data = JsonSerializer.Deserialize<CommandStepIdentity>(json);
            return data;
        }

        public Task SetActiveStep(long chatId, long userId, CommandStepIdentity stepId) {
            var key = GetKey(chatId, userId);
            var options = new DistributedCacheEntryOptions { SlidingExpiration = cacheTime };
            var json = JsonSerializer.Serialize(stepId);
            return cache.SetStringAsync(key, json, options);
        }

        private static string GetKey(long chatId, long userId) => $"{chatId}-{userId}";
    }
}