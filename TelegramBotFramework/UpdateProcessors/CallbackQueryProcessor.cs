using EXRTelegramBotFramework.Contracts;
using Telegram.Bot.Types;

namespace TelegramBotFramework.UpdateProcessors
{
    internal class CallbackQueryProcessor : IUpdateProcessor<CallbackQuery> {
        public Task Process(CallbackQuery data, CancellationToken token) {
            throw new NotImplementedException();
        }
    }
}
