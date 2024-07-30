using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBotFramework {
    internal delegate Task UpdateProcessor(IServiceProvider services, Update update, CancellationToken token);

    internal class UpdateHandler(
        IServiceScopeFactory scopeFactory,
        ILogger<UpdateHandler> logger,
        Dictionary<UpdateType, UpdateProcessor> processors) 
        : IUpdateHandler {
        public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token) {
            using var scope = scopeFactory.CreateScope();

            var hasProcessor = processors.TryGetValue(update.Type, out var processor);
            if (hasProcessor) {
                return processor!(scope.ServiceProvider, update, token);
            }

            logger.LogWarning("Impossible to handle update of type {updateType}", update.Type);
            return Task.CompletedTask;
        }

        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken) {

            return Task.CompletedTask;
        }
    }
}
