using Telegram.Bot.Types.Enums;

namespace TelegramBotFramework.Commands.Utils.Parsers {
    public interface ICommandParser {
        public CommandParsingResult Parse(string text, ChatType chatType);
    }
}
