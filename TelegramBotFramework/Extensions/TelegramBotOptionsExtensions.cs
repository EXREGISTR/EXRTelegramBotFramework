using TelegramBotFramework.UpdateProcessors;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBotFramework {
    public static class TelegramBotOptionsExtensions {
        public static TelegramBotFrameworkOptions AddMessageProcessor(this TelegramBotFrameworkOptions options) {
            options.AddProcessor<Message, MessageProcessor>(
                update => update.Message!, UpdateType.Message);
            return options;
        }

        public static TelegramBotFrameworkOptions AddQueryProcessor(this TelegramBotFrameworkOptions options) {
            options.AddProcessor<CallbackQuery, CallbackQueryProcessor>(
                update => update.CallbackQuery!, UpdateType.CallbackQuery);

            return options;
        }

        // зафигачить и для остальных
    }
}
