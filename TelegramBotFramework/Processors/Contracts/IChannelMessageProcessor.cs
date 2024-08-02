using Telegram.Bot.Types;

namespace TelegramBotFramework.Processors.Contracts {
    public interface IChannelMessageProcessor {
        public Task Process(Message message);
    }
}
