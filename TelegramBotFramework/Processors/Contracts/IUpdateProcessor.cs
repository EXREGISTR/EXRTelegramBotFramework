using Telegram.Bot.Types;

namespace TelegramBotFramework.Processors.Contracts {
    public interface IUpdateProcessor {
        public Task Process(Update update);
    }
}
