using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;

namespace TelegramBotFramework.MessageProcessors {
    public static class MessageProcessors {
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
