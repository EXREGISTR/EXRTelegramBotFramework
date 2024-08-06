using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using TelegramBotFramework.Processors.Contracts;

namespace TelegramBotFramework.Services {
    internal class UpdateHandler(
        IServiceProvider services,
        ILogger<UpdateHandler> logger)
        : IUpdateHandler {
        public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken token) {
            var processor = services.GetRequiredKeyedService<IUpdateProcessor>(update.Type);

            if (processor == null) {
                logger.LogWarning("Impossible to handle update of type {updateType}", update.Type);
                return;
            }

            try {
                await processor.Process(update);
            } catch (Exception exception) {
                HandleError(exception);
            }
        }

        public Task HandlePollingErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken token) {
            if (exception is ApiRequestException requestException) {
                logger.LogError(exception,
                    "Telegram API Error: \n{errorCode}\n{message}",
                    requestException.ErrorCode, requestException.Message);
            } else {
                HandleError(exception);
            }

            return Task.CompletedTask;
        }

        private void HandleError(Exception exception) 
            => logger.LogError(exception, "Error while processing request: \n{message}", exception.Message);
    }
}
