using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace TelegramBotFramework.Tests {
    public class Program {
        public static void Main(string[] args) {
            var builder = Host.CreateApplicationBuilder(args);
            var services = builder.Services;

            services.ConfigureForTypeName<TelegramBotClientOptions>(builder.Configuration);
            services.ConfigureForTypeName<ReceiverOptions>(builder.Configuration);

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