using Telegram.Bot;
using Telegram.Bot.Polling;

namespace TelegramBotFramework.Tests {
    public class Program {
        public static void Main(string[] args) {
            var builder = Host.CreateApplicationBuilder(args);
            var services = builder.Services;

            services.Configure<TelegramBotClientOptions>(
                builder.Configuration.GetSection(nameof(TelegramBotClientOptions)));

            services.Configure<ReceiverOptions>(
                builder.Configuration.GetSection(nameof(ReceiverOptions)));

            services.AddTelegramBot(options => {
                options
                    .AddMessageProcessor()
                    .AddQueryProcessor();
            });

            var host = builder.Build();
            host.Run();
        }
    }
}