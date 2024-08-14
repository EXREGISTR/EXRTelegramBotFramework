using TelegramBotFramework.Commands.Contexts;

namespace TelegramBotFramework.Commands
{
    public interface ICommandStep<TData> where TData : class {
        public Task<CommandStepResult> Execute(CommandStepContext<TData> context);
    }
}
