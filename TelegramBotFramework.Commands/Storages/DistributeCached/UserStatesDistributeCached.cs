using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using TelegramBotFramework.Commands.Storages.Contracts;
using TelegramBotFramework.Storages.Options;

namespace TelegramBotFramework.Commands.Storages.DistributeCached {
    internal class UserStatesDistributeCached(
        IDistributedCache cache,
        CachingOptions options)
        : IUserCommandStatesStorage {
        private readonly DistributedCacheEntryOptions cacheEntryOptions =
            new() { SlidingExpiration = options.CachingTime, };

        public async Task<CommandStepIdentity?> GetActiveStep(long chatId, long userId) {
            string key = GetKey(chatId, userId);

            var json = await cache.GetStringAsync(key);
            if (json == null) return null;

            var data = JsonSerializer.Deserialize<CommandStepIdentity>(json);
            return data;
        }

        public Task SetActiveStep(long chatId, long userId, CommandStepIdentity stepId) {
            var key = GetKey(chatId, userId);
            var json = JsonSerializer.Serialize(stepId);
            return cache.SetStringAsync(key, json, cacheEntryOptions);
        }

        private static string GetKey(long chatId, long userId) => $"{chatId}-{userId}";

        public Task DeleteActiveStep(long chatId, long userId) => cache.RemoveAsync(GetKey(chatId, userId));
    }
}