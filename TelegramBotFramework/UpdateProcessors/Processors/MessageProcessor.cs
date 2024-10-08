﻿using Telegram.Bot.Types;
using TelegramBotFramework.Extensions.ServiceProvider;

namespace TelegramBotFramework.UpdateProcessors.Processors {
    internal class MessageProcessor : UpdateProcessor<Message> {
        protected override async Task Process(Message message, IServiceProvider services, IUpdateProcessor<Message> next) {
            var processor = services.GetMessageProcessor(message.Chat.Type, message.Type);
            await processor.Process(message);

            await next.Process(message, services);
        }
    }
}
