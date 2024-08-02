using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using TelegramBotFramework.Options;

namespace TelegramBotFramework {
    public static class DependencyInjection {
        public static IServiceCollection AddTelegramBot(this IServiceCollection services,
            Action<TelegramBotFrameworkOptions> configurator) {
            var processors = new Dictionary<UpdateType, UpdateProcessor>(6);
            var options = new TelegramBotFrameworkOptions(processors, services);
            configurator(options);

            services.AddHostedService<TelegramMainWorker>();

            services.TryAddSingleton<ITelegramBotClient>(services => {
                var options = services.GetRequiredService<IOptions<TelegramBotClientOptions>>().Value;
                var bot = new TelegramBotClient(options);
                return bot;
            });

            services.TryAddSingleton<IUpdateHandler>(services => {
                var scopeFactory = services.GetRequiredService<IServiceScopeFactory>();
                var logger = services.GetRequiredService<ILogger<UpdateHandler>>();

                var handler = new UpdateHandler(scopeFactory, logger, processors);
                return handler;
            });

            return services;
        }
    }
}
