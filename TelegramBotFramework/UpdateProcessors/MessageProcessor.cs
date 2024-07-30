using Telegram.Bot.Types;
using Microsoft.Extensions.DependencyInjection;
using EXRTelegramBotFramework.Contracts;

namespace TelegramBotFramework.UpdateProcessors
{
    internal class MessageProcessor(IServiceProvider services) : IUpdateProcessor<Message> {
        public Task Process(Message data, CancellationToken token) {
            var chatId = data.Chat.Id;
            var user = data.From;
            if (user == null) {
                var handler = services.GetService<IMessageChannelHandler>();
                if (handler == null) return Task.CompletedTask;


            }
        }
    }
}
