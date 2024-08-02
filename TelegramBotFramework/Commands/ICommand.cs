using TelegramBotFramework.Commands.Contexts;
using TelegramBotFramework.Commands.Results;

namespace TelegramBotFramework.Commands {
    public interface ICommand {
        public Task<CommandResult> Execute(CommandContext context);
    }

    public interface ICommand<TData> where TData : class {
        public Task<CommandResult> Execute(CommandContext<TData> context);
    }
}
