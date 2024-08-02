namespace TelegramBotFramework.Commands.Results {
    public readonly record struct CommandResult {
        private readonly string? error;

        public bool IsSuccess => error == null;
        public bool IsFailure => !IsSuccess;

        public string Error => IsFailure 
            ? error! 
            : throw new NullReferenceException("No error!");

        private CommandResult(string error) { 
            this.error = error; 
        }

        public static readonly CommandResult Success = new();
        public static CommandResult Failure(string message) => new(message);
    }
}
