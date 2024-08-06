using TelegramBotFramework.Commands;

namespace TelegramBotFramework.Exceptions {
    public class CommandStepExecutionException : Exception {
        public CommandStepExecutionException(CommandStepIdentity stepIdentity)
            : base($"An error occurred while executing the command /{stepIdentity.CommandCode} in step number {stepIdentity.Id}") 
            { }

        public CommandStepExecutionException(CommandStepIdentity stepIdentity, string message)
            : base($"An error occurred while executing the command /{stepIdentity.CommandCode} in step number {stepIdentity.Id}. \n" +
                  $"Message: {message}") { }
    }
}
