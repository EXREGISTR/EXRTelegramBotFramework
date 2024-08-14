using Telegram.Bot.Types.Enums;

namespace TelegramBotFramework.Commands.Builders {
    internal record CommandEntry(string Code, CommandType Type, IEnumerable<ChatType> AvailableChatTypes) {
        public const int MaxDescriptionLength = 50;
        public const int MaxHelpLength = 50;

        public string? Description { get; set; }
        public string? Help { get; set; }
    }
}
