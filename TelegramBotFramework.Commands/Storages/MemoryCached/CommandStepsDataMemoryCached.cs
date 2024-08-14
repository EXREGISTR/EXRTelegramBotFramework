using Microsoft.Extensions.Caching.Memory;
using TelegramBotFramework.Commands.Storages.Contracts;
using TelegramBotFramework.Storages.Options;

namespace TelegramBotFramework.Storages.MemoryCached {
    internal class CommandStepsDataMemoryCached(
        IMemoryCache cache,
        CachingOptions options) 
        : ICommandStepsDataStorage {

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
                .SetSlidingExpiration(options.CachingTime); ;

            return Task.CompletedTask;
        }

        private static string GetKey(long chatId, long userId) => $"{chatId}-{userId}";

        public Task DeleteData(long chatId, long userId) {
            cache.Remove(GetKey(chatId, userId));
            return Task.CompletedTask;
        }
    }
}
