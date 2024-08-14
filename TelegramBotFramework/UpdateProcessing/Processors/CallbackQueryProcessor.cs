using Telegram.Bot.Types;

namespace TelegramBotFramework.UpdateProcessing.Processors {
    internal class CallbackQueryProcessor : UpdateProcessor<CallbackQuery> {
        protected override Task Process(CallbackQuery data, IServiceProvider services, IUpdateProcessor<CallbackQuery> next) {
            throw new NotImplementedException();
        }
    }
}
