namespace TelegramBotFramework.Commands {
    public readonly record struct CommandStepResult {
        private readonly string? error;

        public bool IsSuccess => error == null;
        public bool IsFailure => !IsSuccess;

        public string Error => IsFailure
            ? error!
            : throw new NullReferenceException("No error!");

        private CommandStepResult(string error) {
            this.error = error;
        }

        public static readonly CommandStepResult Success = new();
        public static CommandStepResult Failure(string message) => new(message);
    }
}
