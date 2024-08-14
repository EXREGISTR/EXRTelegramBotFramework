using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using TelegramBotFramework.Commands.Contexts;
using TelegramBotFramework.Commands.Proxies.Contracts;
using TelegramBotFramework.Commands.Storages.Contracts;
using TelegramBotFramework.Exceptions;
using TelegramBotFramework.Extensions;

namespace TelegramBotFramework.Commands.Proxies.Steps {
    internal class CommandStepProxy<TData>(
        CommandStepIdentity identity,
        ICommandStepsDataStorage dataStorage,
        IUserCommandStatesStorage userStates,
        IServiceProvider services)
        : ICommandStepProxy where TData : class {
        public async Task Execute(Chat chat, User user, Message message) {
            var chatId = chat.Id;
            var userId = user.Id;

            var data = await dataStorage.GetData<TData>(chatId, userId)
                ?? throw new CommandStepExecutionException(identity, $"No data for user {user.GetUsername()} in {chat.GetTitle()}");

            var context = new CommandStepContext<TData>(chat, user, message, data);
            var step = services.GetRequiredKeyedService<ICommandStep<TData>>(identity);

            try {
                var result = await step.Execute(context).ConfigureAwait(false);
                if (!result.IsSuccess) {
                    throw new CommandStepExecutionException(identity, result.Error);
                }
            } catch (Exception exception) {
                throw new CommandStepExecutionException(identity, exception.Message);
            }

            var nextIdentity = CommandStepIdentity.Next(identity);

            await Task.WhenAll(
                userStates.SetActiveStep(chatId, userId, nextIdentity),
                dataStorage.SetData(chatId, userId, data)).ConfigureAwait(false);
        }
    }
}
