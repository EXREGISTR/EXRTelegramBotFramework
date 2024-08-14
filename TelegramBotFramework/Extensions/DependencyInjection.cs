using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using TelegramBotFramework.Services;

namespace TelegramBotFramework {
    public static class DependencyInjection {
        public static IServiceCollection AddTelegramBot(this IServiceCollection services) {
            services.AddHostedService<TelegramMessageReceiver>();

            services.TryAddSingleton<ITelegramBotClient>(services => {
                var options = services.GetRequiredService<IOptions<TelegramBotClientOptions>>().Value;
                var bot = new TelegramBotClient(options);
                return bot;
            });

            services.TryAddSingleton<IUpdateHandler, UpdateHandler>();
            return services;
        }
    }
}
