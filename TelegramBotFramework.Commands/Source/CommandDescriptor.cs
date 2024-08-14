using Telegram.Bot.Types.Enums;

namespace TelegramBotFramework.Commands {
    internal enum CommandType {
        Common, Parameterized, StepByStep
    }

    internal record CommandDescriptor(
        string Code,
        string? Description,
        CommandType Type,
        IEnumerable<ChatType> AvailableChatTypes);
}
