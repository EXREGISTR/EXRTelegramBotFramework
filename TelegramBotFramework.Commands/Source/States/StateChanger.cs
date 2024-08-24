namespace TelegramBotFramework.Commands.States {
    internal class StateChanger : ICommandStateChanger {
        internal CommandStateIdentity? NextIdentity { get; private set; }
        internal bool NeedToEndHandling { get; private set; }

        public void FinishHandling() => NeedToEndHandling = true;

        public void Next<TState>() where TState : class, ICommandState
            => NextIdentity = CommandStateIdentity.Create(typeof(TState));
    }

    internal class StateChanger<TData> : ICommandStateChanger<TData> where TData : class {
        internal CommandStateIdentity? NextIdentity { get; private set; }
        internal bool FinishWorkWithUser { get; private set; }

        public void FinishHandling() => FinishWorkWithUser = true;

        public void Next<TState>() where TState : class, ICommandState<TData>
            => NextIdentity = CommandStateIdentity.Create(typeof(TState));
    }
}
