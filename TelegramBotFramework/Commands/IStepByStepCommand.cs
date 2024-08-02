using TelegramBotFramework.Commands.Contexts;
using TelegramBotFramework.Commands.Results;

namespace TelegramBotFramework.Commands {
    public interface IStepByStepCommand<TData> where TData : class {
        public Task<CommandResult> Execute(CommandContext<TData> Data);
    }
}
