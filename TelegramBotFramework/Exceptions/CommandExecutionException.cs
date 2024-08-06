namespace TelegramBotFramework.Exceptions {
    public class CommandExecutionException : Exception {
        public CommandExecutionException(string commandCode)
            : base($"An error occurred while executing the command /{commandCode}") { }

        public CommandExecutionException(string commandCode, string reason)
            : base($"An error occurred while executing the command /{commandCode}.\n" +
                  $"Reason: \n" +
                  $"{reason}") { }
    }
}
