using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;
using TelegramBotFramework.Processors;
using TelegramBotFramework.Processors.Contracts;
using TelegramBotFramework.Storages.Contracts;
using TelegramBotFramework.Storages.DistributeCached;
using TelegramBotFramework.Storages.MemoryCached;

namespace TelegramBotFramework.Options {
    public class TelegramBotFrameworkOptions {
        private readonly IServiceCollection services;

        internal TelegramBotFrameworkOptions(IServiceCollection services) {
            this.services = services;
        }

        public void AddMessageProcessor<TImplementation>(MessageType messageType, IEnumerable<ChatType> availableChatTypes)
            where TImplementation : class, IMessageProcessor {
            foreach (var chatType in availableChatTypes) {
                var identity = new ProcessorIdentity(chatType, messageType);
                services.AddKeyedScoped<IMessageProcessor, TImplementation>(identity);
            }
        }

        public void AddUpdateProcessor<TImplementation>(UpdateType type)
            where TImplementation : class, IUpdateProcessor 
            => services.AddKeyedSingleton<IUpdateProcessor, TImplementation>(type);

        public void UseMemoryCachedStorages(Action<MemoryCacheOptions>? configurator = null) {
            if (configurator != null) {
                services.AddMemoryCache(configurator);
            } else {
                services.AddMemoryCache();
            }

            services.AddSingleton<ICommandStepsDataStorage, CommandStepsDataMemoryCached>();
            services.AddSingleton<IUserStatesStorage, UserStatesMemoryCached>();
        }

        public void UseDistributeCachedStorages(Action<MemoryDistributedCacheOptions>? configurator = null) {
            if (configurator != null) {
                services.AddDistributedMemoryCache(configurator);
            } else {
                services.AddDistributedMemoryCache();
            }

            services.AddSingleton<ICommandStepsDataStorage, CommandStepsDataDistributeCached>();
            services.AddSingleton<IUserStatesStorage, UserStatesDistributeCached>();
        }
    }
}
