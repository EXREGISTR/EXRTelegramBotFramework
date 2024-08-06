using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using TelegramBotFramework.Commands.Contexts;
using TelegramBotFramework.Commands.Proxies.Contracts;
using TelegramBotFramework.Exceptions;
using TelegramBotFramework.Storages.Contracts;

namespace TelegramBotFramework.Commands.Proxies.Steps {
    internal class LastCommandStepProxy<TData>(
        CommandStepIdentity identity,
        ICommandStepsDataStorage dataStorage,
        IUserStatesStorage userStates,
        IServiceProvider services)
        : ICommandStepProxy where TData : class {
        public async Task Execute(Chat chat, User sender, Message message) {
            var chatId = chat.Id;
            var senderId = sender.Id;

            var data = await dataStorage.GetData<TData>(chatId, senderId)
                ?? throw new NullReferenceException($"No data for user {senderId}");

            var context = new CommandStepContext<TData>(chat, sender, message, data);
            var lastStep = services.GetRequiredKeyedService<ICommandStep<TData>>(identity);

            try {
                var result = await lastStep.Execute(context);
                if (!result.IsSuccess) {
                    throw new CommandStepExecutionException(identity, result.Error);
                }
            } catch (Exception exception) {
                throw new CommandStepExecutionException(identity, exception.Message);
            }

            await Handle(chat, sender, data).ConfigureAwait(false);
            await EndHandling(chatId, senderId).ConfigureAwait(false);
        }

        private Task Handle(Chat chat, User sender, TData data) {
            var handler = services.GetRequiredKeyedService<ICommand<TData>>(identity.CommandCode);
            var commandContext = new CommandContext<TData>(chat, sender, data);

            try {
                return handler.Execute(commandContext);
            } catch (Exception exception) {
                throw new CommandExecutionException(identity.CommandCode, exception.Message);
            }
        }

        private Task EndHandling(long chatId, long senderId) {
            var stateDeletingTask = userStates.DeleteActiveStep(chatId, senderId);
            var dataDeletingTask = dataStorage.DeleteData(chatId, senderId);

            return Task.WhenAll(stateDeletingTask, dataDeletingTask);
        }
    }
}
