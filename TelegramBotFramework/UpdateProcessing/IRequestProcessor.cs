using Telegram.Bot.Types;

namespace TelegramBotFramework.UpdateProcessing {
    internal interface IRequestProcessor {
        public Task Process(IServiceProvider services, Update update);
    }
}
