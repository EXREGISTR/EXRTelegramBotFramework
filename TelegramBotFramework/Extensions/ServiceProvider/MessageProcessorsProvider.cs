using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;

namespace TelegramBotFramework.Extensions.ServiceProvider {
    internal static class MessageProcessorsProvider {
        public static IMessageProcessor GetMessageProcessor(this IServiceProvider services,
            ChatType chatType, MessageType messageType) {
            var identity = new MessageProcessorIdentity(chatType, messageType);
            return services.GetRequiredKeyedService<IMessageProcessor>(identity);
        }
    }
}
