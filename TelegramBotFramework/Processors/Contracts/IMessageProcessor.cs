using Telegram.Bot.Types;

namespace TelegramBotFramework.Processors.Contracts {
    public interface IMessageProcessor {
        public Task Process(Message data);
    }
}
