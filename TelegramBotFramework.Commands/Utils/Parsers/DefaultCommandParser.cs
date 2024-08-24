using Telegram.Bot.Types.Enums;
using TelegramBotFramework.Commands.Exceptions;

namespace TelegramBotFramework.Commands.Utils.Parsers {
    internal class DefaultCommandParser(string botUsername) : ICommandParser {
        public CommandParsingResult Parse(string input, ChatType chatType) {
            if (!input.StartsWith('/')) throw new CommandSignatureException("Command should be starts with '/'");

            string command;
            string[] arguments;

            if (chatType == ChatType.Private) {
                string[] parts = input.TrimStart('/').Split(' ', StringSplitOptions.RemoveEmptyEntries);
                command = parts[0];
                arguments = parts.Length > 1 ? parts[1..] : [];
            } else {
                string[] parts = input.TrimStart('/').Split(' ', 2);
                string commandWithBotTag = parts[0];

                if (commandWithBotTag.Contains('@')) {
                    var splitCommand = commandWithBotTag.Split('@');
                    command = splitCommand[0];
                    string? botTag = splitCommand.Length > 1 ? splitCommand[1] : null;

                    if (botTag != botUsername) {
                        throw new CommandSignatureException($"Bot can not handle command for another bot {botTag}");
                    }
                } else {
                    command = commandWithBotTag;
                }

                arguments = parts.Length > 1 ? parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries) : [];
            }

            return new(command, arguments);
        }
    }
}
