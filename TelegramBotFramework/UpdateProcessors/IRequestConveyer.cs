namespace TelegramBotFramework.UpdateProcessors {
    public interface IRequestConveyer<TData> {
        public IRequestConveyer<TData> UseProcessor(UpdateProcessor<TData> processor);
    }
}
