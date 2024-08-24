namespace TelegramBotFramework.Commands {
    public interface ICommand {
        public Task Execute(CommandContext context);
    }

    public interface ICommand<TData> {
        public Task Execute(CommandContext<TData> context);
    }
}
