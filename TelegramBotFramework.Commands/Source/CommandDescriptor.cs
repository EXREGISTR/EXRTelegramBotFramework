using Telegram.Bot.Types.Enums;

namespace TelegramBotFramework.Commands {
    internal enum CommandType {
        Common, Parameterized, Stateless
    }

    internal record CommandDescriptor(
        string Code,
        string? Help,
        CommandType Type,
        IEnumerable<ChatType> AvailableChatTypes);
}
