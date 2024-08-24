using Telegram.Bot.Types;

namespace TelegramBotFramework {
    public interface IUpdateProcessor<TData> {
        public Task Process(TData data, IServiceProvider services);
    }

    public interface IUpdateProcessor {
        public Task Process(Update update, IServiceProvider services);
    }
}
