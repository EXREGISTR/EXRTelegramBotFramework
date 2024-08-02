using TelegramBotFramework.Commands.Contexts;
using TelegramBotFramework.Commands.Results;

namespace TelegramBotFramework.Commands {
    public interface ICommandStep<TData> where TData : class {
        public Task<CommandResult> Execute(CommandStepContext<TData> context);
    }

    public record CommandStepIdentity(string CommandCode, int Id);
}
