using Telegram.Bot.Types.Enums;

namespace TelegramBotFramework.MessageProcessors {
    internal record MessageProcessorIdentity(ChatType ChatType, MessageType MessageType);
}
