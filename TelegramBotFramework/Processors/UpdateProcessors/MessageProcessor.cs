using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotFramework.Commands;
using TelegramBotFramework.Processors.Contracts;
using TelegramBotFramework.Storages.Contracts;

namespace TelegramBotFramework.Processors.UpdateProcessors {
    internal class MessageProcessor(
        IUserStatesStorage userStatesStorage,
        IServiceScopeFactory scopeFactory)
        : IUpdateProcessor {
        public async Task Process(Update update) {
            var message = update.Message!;
            var services = scopeFactory.CreateScope().ServiceProvider;

            var chatType = message.Chat.Type;

            if (chatType == ChatType.Channel) {
                await ProcessMessage(message, services);
                return;
            }

            var currentCommandStep = await userStatesStorage.GetActiveStep(message.Chat.Id, message.From!.Id);

            if (currentCommandStep != null) {
                await ProcessCommand(message, stepId: currentCommandStep, services);
                return;
            }

            var isCommand = message.Type == MessageType.Text && message.Text!.StartsWith('/');

            if (isCommand) {
                await ProcessCommand(message, stepId: null, services);
            } else {
                await ProcessMessage(message, services);
            }
        }

        private static Task ProcessMessage(Message message, IServiceProvider services) {
            var identity = new ProcessorIdentity(message.Chat.Type, message.Type);
            var channelMessageProcessor =
                services.GetRequiredKeyedService<IMessageProcessor>(identity);

            return channelMessageProcessor.Process(message);
        }

        private static Task ProcessCommand(Message message, CommandStepIdentity? stepId, IServiceProvider services) {
            var commandProcessor = services.GetRequiredService<CommandProcessor>();

            if (stepId == null) {
                return commandProcessor.Process(message);
            } 

            return commandProcessor.Process(message, stepId);
        }
    }
}
