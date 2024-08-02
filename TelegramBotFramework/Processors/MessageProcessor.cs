using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using TelegramBotFramework.Processors.Contracts;
using TelegramBotFramework.Storages.Contracts;

namespace TelegramBotFramework.Processors {
    internal class MessageProcessor(
        IUserStatesStorage userStatesStorage,
        ICommandExecutor commandExecutor,
        IServiceScopeFactory scopeFactory)
        : IUpdateProcessor<Message> {
        public async Task Process(Message message) {
            var services = scopeFactory.CreateScope().ServiceProvider;
            if (message.From == null) {
                var channelMessageProcessor = services.GetRequiredKeyedService<IChannelMessageProcessor>(message.Type);
                await channelMessageProcessor.Process(message);
                return;
            }

            var currentStep = await userStatesStorage.GetActiveStep(message.Chat.Id, message.From.Id);
            if (currentStep == null) {
                if (message.Text != null && message.Text.StartsWith('/')) {
                    await commandExecutor.Execute(message);
                    return;
                }

                var messageProcessor = services.GetRequiredKeyedService<IMessageProcessor>(message.Type);
                await messageProcessor.Process(message);
            } else {
                await commandExecutor.Execute(message, currentStep);
            }
        }
    }
}
