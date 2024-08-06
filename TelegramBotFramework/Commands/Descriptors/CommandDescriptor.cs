using Telegram.Bot.Types.Enums;

namespace TelegramBotFramework.Commands.Descriptors {
    internal enum CommandType { 
        Common, Parameterized, StepByStep
    }

    internal record CommandDescriptor(
        string Code,
        string Description,
        CommandType Type,
        ChatType[] AvailableChatTypes);
}
