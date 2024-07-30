using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Polling;
using Telegram.Bot;
using Microsoft.Extensions.Options;

namespace TelegramBotFramework {
    public static class DependencyInjection {
        public static IServiceCollection AddTelegramBot(this IServiceCollection services,
            Action<TelegramBotFrameworkOptions> configurator) {
            var options = new TelegramBotFrameworkOptions(services);
            configurator(options);

            services.AddHostedService<TelegramMainWorker>();

            services.AddSingleton<ITelegramBotClient, TelegramBotClient>(services => {
                var options = services.GetRequiredService<IOptions<TelegramBotClientOptions>>().Value;
                var bot = new TelegramBotClient(options);
                return bot;
            });

            services.AddSingleton<IUpdateHandler, UpdateHandler>(services => {
                var scopeFactory = services.GetRequiredService<IServiceScopeFactory>();
                var logger = services.GetRequiredService<ILogger<UpdateHandler>>();
                var processors = options.Processors;

                var handler = new UpdateHandler(scopeFactory, logger, processors);
                return handler;
            });

            return services;
        }
    }
}
