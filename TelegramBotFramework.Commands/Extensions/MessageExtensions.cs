using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBotFramework.Commands.Extensions {
    public static class MessageExtensions {
        public static bool IsCommand(this Message message) {
            var isCommand = message.Type == MessageType.Text && message.Text!.StartsWith('/');
            return isCommand;
        }
    }
}
