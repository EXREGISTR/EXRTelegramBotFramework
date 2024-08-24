using Telegram.Bot.Types.Enums;
using TelegramBotFramework.Commands.Utils.Parsers;

namespace TelegramBotFramework.Commands.Utils.Parsers.Contracts
{
    public interface ICommandParser
    {
        public CommandParsingResult Parse(string text, ChatType chatType);
    }
}
