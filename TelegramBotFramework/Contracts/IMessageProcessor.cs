using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBotFramework {
    /// <summary>
    /// The processor handles a specific type <see cref="MessageType"/> of message. 
    /// Exists within the scope of the request
    /// </summary>
    public interface IMessageProcessor {
        public Task Process(Message message);
    }
}
