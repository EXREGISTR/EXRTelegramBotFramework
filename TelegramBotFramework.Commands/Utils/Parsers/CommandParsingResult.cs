namespace TelegramBotFramework.Commands.Utils.Parsers {
    public readonly record struct CommandParsingResult(
        string Code, string[] Arguments);
}
