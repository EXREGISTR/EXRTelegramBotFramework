using Microsoft.Extensions.Caching.Memory;
using TelegramBotFramework.Commands;
using TelegramBotFramework.Storages.Contracts;
using TelegramBotFramework.Storages.Options;

namespace TelegramBotFramework.Storages.MemoryCached {
    internal class UserStatesMemoryCached(
        IMemoryCache cache,
        CachingOptions options) : IUserStatesStorage {
        private readonly TimeSpan cacheTime = TimeSpan.FromMinutes(options.CacheTimeInMinutes);

        public Task<CommandStepIdentity?> GetActiveStep(long chatId, long userId) {
            string key = GetKey(chatId, userId);

            var stepId = cache.Get<CommandStepIdentity?>(key);
            return Task.FromResult(stepId);
        }

        public Task SetActiveStep(long chatId, long userId, CommandStepIdentity stepId) {
            var key = GetKey(chatId, userId);
            cache
                .CreateEntry(key)
                .SetValue(stepId)
                .SetSlidingExpiration(cacheTime);

            return Task.CompletedTask;
        }

        private static string GetKey(long chatId, long userId) => $"{chatId}-{userId}";
    }
}
