namespace TelegramBotFramework.Commands.Exceptions {
    public class CommandCannotBeProcessedException(string commandCode, string reason)
        : Exception {
        public string CommandCode { get; init; } = commandCode;
        public string Reason { get; init; } = reason;
    }
}
