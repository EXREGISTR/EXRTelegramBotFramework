namespace TelegramBotFramework.UpdateProcessing {
    public interface IUpdateProcessor<TData> {
        public Task Process(TData data, IServiceProvider services);
    }
}
