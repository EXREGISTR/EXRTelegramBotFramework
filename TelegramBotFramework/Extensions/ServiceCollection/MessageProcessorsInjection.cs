using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;

namespace TelegramBotFramework {
    internal record struct MessageProcessorIdentity(ChatType ChatType, MessageType MessageType);

    public static class MessageProcessorsInjection {
        public static IServiceCollection AddMessageProcessor<TImplementation>(
            this IServiceCollection services,
            MessageType messageType,
            IEnumerable<ChatType> availableChatTypes)
            where TImplementation : class, IMessageProcessor {
            foreach (var chatType in availableChatTypes) {
                var identity = new MessageProcessorIdentity(chatType, messageType);
                services.AddKeyedScoped<IMessageProcessor, TImplementation>(identity);
            }

            return services;
        }
    }
}
