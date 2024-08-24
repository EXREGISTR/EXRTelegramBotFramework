namespace TelegramBotFramework.Commands.States {
    public interface ICommandStateChanger {
        public void Next<TState>() where TState : class, ICommandState;
        public void FinishHandling();
    }

    public interface ICommandStateChanger<TData> where TData : class {
        public void Next<TState>() where TState : class, ICommandState<TData>;
        public void FinishHandling();
    }
}
