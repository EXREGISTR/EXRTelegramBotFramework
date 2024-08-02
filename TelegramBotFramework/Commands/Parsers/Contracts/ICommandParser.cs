using Telegram.Bot.Types.Enums;

namespace TelegramBotFramework.Commands.Parsers.Contracts {
    public interface ICommandParser {
        public CommandParsingResult Parse(string text, ChatType chatType);
    }
}
