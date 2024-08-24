using Telegram.Bot.Types;

namespace TelegramBotFramework.UpdateProcessors {
    internal sealed class RequestConveyer<TData>(Func<Update, TData> dataSelector)
        : IRequestConveyer<TData>, IUpdateProcessor {
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

        public Task Process(Update update, IServiceProvider services) {
            if (first == null) {
                throw new NullReferenceException(
                    $"You are tracking the update {update.Type}, but you are not processing it");
            }

            var data = dataSelector(update);
            return first.Process(data, services);
        }
    }
}
