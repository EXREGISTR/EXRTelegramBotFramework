namespace TelegramBotFramework.Commands.States {
    /// <summary>
    /// Handler before entering to the state <see cref="ICommandState"/>
    /// </summary>
    public interface IBeforeEnteringStateHandler {
        public Task BeforeEnter(CommandStateContext context);
    }

    /// <summary>
    /// Handler before entering to the state <see cref="ICommandState{TData}"/>
    /// </summary>
    public interface IBeforeEnteringStateHandler<TData> {
        public Task BeforeEnter(CommandStateContext<TData> context);
    }
}
