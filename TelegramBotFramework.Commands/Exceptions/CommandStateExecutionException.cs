namespace TelegramBotFramework.Commands.Exceptions {
    internal class CommandStateExecutionException : Exception {
        public CommandStateExecutionException(CommandStateIdentity stepIdentity)
            : base($"An error occurred while executing the command state {stepIdentity.Key}") { }

        public CommandStateExecutionException(CommandStateIdentity stepIdentity, string message)
            : base($"An error occurred while executing the command state {stepIdentity.Key}. \n" +
                  $"Message: {message}") { }
    }
}
