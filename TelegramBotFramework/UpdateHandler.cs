using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBotFramework {
    internal class UpdateHandler(
        IServiceScopeFactory scopeFactory,
        ILogger<UpdateHandler> logger,
        Dictionary<UpdateType, UpdateProcessor> processors) 
        : IUpdateHandler {
        public Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken token) {
            using var scope = scopeFactory.CreateScope();

            var hasProcessor = processors.TryGetValue(update.Type, out var processor);
            if (hasProcessor) {
                return processor!(scope.ServiceProvider, update);
            }

            logger.LogWarning("Impossible to handle update of type {updateType}", update.Type);
            return Task.CompletedTask;
        }

        public Task HandlePollingErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken token) {
            if (exception is ApiRequestException requestException) {
                logger.LogError(exception, 
                    "Telegram API Error: \n{errorCode}\n{message}", 
                    requestException.ErrorCode, requestException.Message);
            } else {
                logger.LogError(exception, "Error while processing {message}", exception.Message);
            }
            return Task.CompletedTask;
        }
    }
}
