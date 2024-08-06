using Telegram.Bot.Types.Enums;

namespace TelegramBotFramework.Processors {
    public record ProcessorIdentity(ChatType ChatType, MessageType MessageType);
}
