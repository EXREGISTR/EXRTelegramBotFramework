namespace TelegramBotFramework.UpdateProcessing {
    public interface IRequestConveyer<TData> {
        public IRequestConveyer<TData> UseProcessor(UpdateProcessor<TData> processor);
    }
}
