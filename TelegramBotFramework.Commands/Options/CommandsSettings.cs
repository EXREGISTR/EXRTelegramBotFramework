using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using TelegramBotFramework.Commands.Storages;
using TelegramBotFramework.Commands.Storages.Cached;

namespace TelegramBotFramework.Commands.Options
{
    public class CommandsSettings {
        private readonly IServiceCollection services;

        internal CommandsSettings(IServiceCollection services) {
            this.services = services;
        }

        public void UseMemoryCachedStorages(Action<MemoryCacheOptions>? configurator = null) {
            if (configurator != null) {
                services.AddMemoryCache(configurator);
            } else {
                services.AddMemoryCache();
            }

            services.AddSingleton(typeof(ICommandsDataStorage), typeof(MemoryCachedStatesStorage));
        }

        public void UseDistributeCachedStorages(Action<MemoryDistributedCacheOptions>? configurator = null) {
            if (configurator != null) {
                services.AddDistributedMemoryCache(configurator);
            } else {
                services.AddDistributedMemoryCache();
            }

            services.AddSingleton(typeof(ICommandsDataStorage), typeof(DistributeCachedStatesStorage));
        }
    }
}
