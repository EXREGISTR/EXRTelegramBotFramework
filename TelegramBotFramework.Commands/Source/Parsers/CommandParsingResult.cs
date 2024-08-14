namespace TelegramBotFramework.Commands.Parsers {
    public readonly record struct CommandParsingResult(
        string Code, string[] Arguments);
}
