using Telegram.Bot.Types;

namespace TelegramBotFramework.MessageProcessors
{
    public interface IMessageProcessor
    {
        public Task Process(Message message);
    }
}
