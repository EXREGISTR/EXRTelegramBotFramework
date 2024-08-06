using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using TelegramBotFramework.Options;
using TelegramBotFramework.Processors.Contracts;
using TelegramBotFramework.Processors.UpdateProcessors;
using TelegramBotFramework.Services;

namespace TelegramBotFramework {
    public static class DependencyInjection {
        public static IServiceCollection AddTelegramBot(this IServiceCollection services,
            Action<TelegramBotFrameworkOptions> configurator) {
            var options = new TelegramBotFrameworkOptions(services);
            configurator(options);

            services.AddHostedService<TelegramMessageReceiver>();

            services.TryAddSingleton<ITelegramBotClient>(services => {
                var options = services.GetRequiredService<IOptions<TelegramBotClientOptions>>().Value;
                var bot = new TelegramBotClient(options);
                return bot;
            });

            services.TryAddSingleton<IUpdateHandler, UpdateHandler>();
            services.TryAddKeyedSingleton<IUpdateProcessor, MessageProcessor>(UpdateType.Message);
            services.TryAddKeyedSingleton<IUpdateProcessor, CallbackQueryProcessor>(UpdateType.CallbackQuery);
            return services;
        }
    }
}
