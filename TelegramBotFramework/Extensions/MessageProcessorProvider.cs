using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;
using TelegramBotFramework.MessageProcessors;

namespace TelegramBotFramework.Extensions
{
    internal static class MessageProcessorProvider
    {
        public static IMessageProcessor GetMessageProcessor(this IServiceProvider services, ChatType chatType, MessageType messageType)
        {
            var identity = new MessageProcessorIdentity(chatType, messageType);
            return services.GetRequiredKeyedService<IMessageProcessor>(identity);
        }
    }
}
