using Telegram.Bot.Types;
using TelegramBotFramework.UpdateProcessors;
using TelegramBotFramework.UpdateProcessors.Processors;

namespace TelegramBotFramework.Extensions {
    public static class RequestConveyerExtensions {
        public static IRequestConveyer<Message> UseMessageProcessor(this IRequestConveyer<Message> conveyer) {
            return conveyer.UseProcessor(new MessageProcessor());
        }
    }
}
