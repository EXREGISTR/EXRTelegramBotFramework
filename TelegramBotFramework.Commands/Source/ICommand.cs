using TelegramBotFramework.Commands.Contexts;

namespace TelegramBotFramework.Commands {
    public interface ICommand {
        public Task Execute(CommandContext context);
    }

    public interface ICommand<TData> where TData : class {
        public Task Execute(CommandContext<TData> context);
    }
}
