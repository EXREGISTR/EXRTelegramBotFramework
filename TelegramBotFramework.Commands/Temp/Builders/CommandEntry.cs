using Telegram.Bot.Types.Enums;

namespace TelegramBotFramework.Commands.Temp.Builders
{
    internal class CommandEntry(CommandType type)
    {
        public readonly List<ChatType> AvailableChatTypes = [];
        public readonly CommandType Type = type;
        public string? Code { get; set; }
        public string? Help { get; set; }
    }
}
