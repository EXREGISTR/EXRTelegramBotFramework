using Microsoft.Extensions.Caching.Memory;
using TelegramBotFramework.Commands;
using TelegramBotFramework.Commands.Storages.Contracts;
using TelegramBotFramework.Storages.Options;

namespace TelegramBotFramework.Storages.MemoryCached {
    internal class UserStatesMemoryCached(
        IMemoryCache cache,
        CachingOptions options) 
        : IUserCommandStatesStorage {
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
                .SetSlidingExpiration(options.CachingTime);

            return Task.CompletedTask;
        }

        private static string GetKey(long chatId, long userId) => $"{chatId}-{userId}";

        public Task DeleteActiveStep(long chatId, long userId) {
            cache.Remove(GetKey(chatId, userId));
            return Task.CompletedTask;
        }
    }
}
