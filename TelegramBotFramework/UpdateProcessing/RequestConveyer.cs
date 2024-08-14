using Telegram.Bot.Types;

namespace TelegramBotFramework.UpdateProcessing {
    internal sealed class RequestConveyer<TData>(Func<Update, TData> dataSelector)
        : IRequestConveyer<TData>, IRequestProcessor {
        private UpdateProcessor<TData>? first;
        private UpdateProcessor<TData> last = null!;

        public IRequestConveyer<TData> UseProcessor(UpdateProcessor<TData> processor) {
            if (first == null) {
                first = processor;
                last = processor;
                return this;
            }

            last.SetNextProcessor(processor);
            last = processor;
            return this;
        }

        public Task Process(IServiceProvider services, Update update) {
            if (first == null) {
                throw new NullReferenceException();
            }

            var data = dataSelector(update);
            return first.Process(data, services);
        }
    }
}
