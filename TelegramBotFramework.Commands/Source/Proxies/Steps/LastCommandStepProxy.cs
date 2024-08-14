using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using TelegramBotFramework.Commands.Contexts;
using TelegramBotFramework.Commands.Proxies.Contracts;
using TelegramBotFramework.Commands.Storages.Contracts;
using TelegramBotFramework.Exceptions;

namespace TelegramBotFramework.Commands.Proxies.Steps {
    internal class LastCommandStepProxy<TData>(
        CommandStepIdentity identity,
        ICommandStepsDataStorage dataStorage,
        IUserCommandStatesStorage userStates,
        IServiceProvider services)
        : ICommandStepProxy where TData : class {
        public async Task Execute(Chat chat, User sender, Message message) {
            var chatId = chat.Id;
            var senderId = sender.Id;

            var data = await dataStorage.GetData<TData>(chatId, senderId)
                ?? throw new CommandStepExecutionException(identity, $"No data for user {senderId}");

            var context = new CommandStepContext<TData>(chat, sender, message, data);
            var lastStep = services.GetRequiredKeyedService<ICommandStep<TData>>(identity);

            try {
                var result = await lastStep.Execute(context).ConfigureAwait(false);
                if (!result.IsSuccess) {
                    throw new CommandStepExecutionException(identity, result.Error);
                }
            } catch (Exception exception) {
                throw new CommandStepExecutionException(identity, exception.Message);
            }

            await SendDataForCommand(chat, sender, data).ConfigureAwait(false);

            await Task.WhenAll(
                userStates.DeleteActiveStep(chatId, senderId),
                dataStorage.DeleteData(chatId, senderId)).ConfigureAwait(false);
        }

        private Task SendDataForCommand(Chat chat, User sender, TData data) {
            var handler = services.GetRequiredKeyedService<ICommand<TData>>(identity.CommandCode);
            var commandContext = new CommandContext<TData>(chat, sender, data);

            try {
                return handler.Execute(commandContext);
            } catch (Exception exception) {
                throw new CommandExecutionException(identity.CommandCode, exception.Message);
            }
        }
    }
}
