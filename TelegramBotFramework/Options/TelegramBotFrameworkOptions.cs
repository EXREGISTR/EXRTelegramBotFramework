using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotFramework.Processors.Contracts;
using TelegramBotFramework.Storages.Contracts;
using TelegramBotFramework.Storages.DistributeCached;
using TelegramBotFramework.Storages.MemoryCached;

namespace TelegramBotFramework.Options
{
    public class TelegramBotFrameworkOptions {
        private readonly Dictionary<UpdateType, UpdateProcessor> processors;
        private readonly IServiceCollection services;

        internal TelegramBotFrameworkOptions(
            Dictionary<UpdateType, UpdateProcessor> processors,
            IServiceCollection services) {
            this.processors = processors;
            this.services = services;
        }

        internal bool AddProcessor<TUpdateData, TImplementation>(
            Func<Update, TUpdateData> dataSelector, UpdateType updateType)
            where TImplementation : class, IUpdateProcessor<TUpdateData> {
            services.TryAddScoped<IUpdateProcessor<TUpdateData>, TImplementation>();
            return processors.TryAdd(updateType, ProcessorWrapper);

            Task ProcessorWrapper(IServiceProvider services, Update update) {
                var updateData = dataSelector(update);
                var processor = services.GetRequiredService<IUpdateProcessor<TUpdateData>>();
                return processor.Process(updateData);
            }
        }

        public void UseMemoryCachingStorages(Action<MemoryCacheOptions>? configurator = null) {
            if (configurator != null) {
                services.AddMemoryCache(configurator);
            } else {
                services.AddMemoryCache();
            }

            services.TryAddSingleton<ICommandStepsDataStorage, CommandStepsDataMemoryCached>();
            services.TryAddSingleton<IUserStatesStorage, UserStatesMemoryCached>();
        }

        public void UseDistributedCachingStorages(Action<MemoryDistributedCacheOptions>? configurator = null) {
            if (configurator != null) {
                services.AddDistributedMemoryCache(configurator);
            } else {
                services.AddDistributedMemoryCache();
            }

            services.TryAddSingleton<ICommandStepsDataStorage, CommandStepsDataDistributeCached>();
            services.TryAddSingleton<IUserStatesStorage, UserStatesDistributeCached>();
        }
    }
}
