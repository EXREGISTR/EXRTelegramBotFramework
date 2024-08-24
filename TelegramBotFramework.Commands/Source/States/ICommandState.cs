namespace TelegramBotFramework.Commands.States {
    public interface ICommandState {
        public Task Execute(CommandStateContext context, ICommandStateChanger stateChanger);
    }

    public interface ICommandState<TData> where TData : class {
        public Task Execute(CommandStateContext<TData> context, ICommandStateChanger<TData> stateChanger);
    }
}
