namespace TelegramBotFramework {
    public abstract class UpdateProcessor<TData> : IUpdateProcessor<TData>
    {
        private static readonly IUpdateProcessor<TData> defaultProcessor = new DefaultProcessor();
        private class DefaultProcessor : IUpdateProcessor<TData>
        {
            public Task Process(TData data, IServiceProvider services) => Task.CompletedTask;
        }

        private IUpdateProcessor<TData>? next;

        internal void SetNextProcessor(IUpdateProcessor<TData>? next) => this.next = next;

        public Task Process(TData data, IServiceProvider services)
            => Process(data, services, next ?? defaultProcessor);

        protected abstract Task Process(TData data, IServiceProvider services, IUpdateProcessor<TData> next);
    }
}
