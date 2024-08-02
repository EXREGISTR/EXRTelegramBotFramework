using Telegram.Bot.Types.Enums;

namespace TelegramBotFramework.Commands.Descriptors {
    internal record CommandDescriptor<TProxy>(
        string Code,
        string Description,
        TProxy Proxy,
        ChatType[] AvailableChatTypes);
}
