using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace TelegramBotFramework.Services {
    internal class TelegramMessageReceiver(
        ITelegramBotClient bot,
        IUpdateHandler handler,
        IOptions<ReceiverOptions> receiverOptions)
        : BackgroundService {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                await bot.ReceiveAsync(handler, receiverOptions.Value, stoppingToken).ConfigureAwait(false);
            }
        }
    }
}
