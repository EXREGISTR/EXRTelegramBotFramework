using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using TelegramBotFramework.Commands.Contexts;
using TelegramBotFramework.Commands.Proxies.Contracts;
using TelegramBotFramework.Exceptions;
using TelegramBotFramework.Extensions;
using TelegramBotFramework.Storages.Contracts;

namespace TelegramBotFramework.Commands.Proxies.Steps {
    internal class CommandStepProxy<TData>(
        CommandStepIdentity identity,
        ICommandStepsDataStorage dataStorage,
        IUserStatesStorage userStates,
        IServiceProvider services)
        : ICommandStepProxy where TData : class {
        public async Task Execute(Chat chat, User user, Message message) {
            var chatId = chat.Id;
            var userId = user.Id;

            var data = await dataStorage.GetData<TData>(chatId, userId)
                ?? throw new NullReferenceException($"No data for user {user.GetUsername()} in {chat.GetName()}");

            var context = new CommandStepContext<TData>(chat, user, message, data);
            var step = services.GetRequiredKeyedService<ICommandStep<TData>>(identity);

            try {
                var result = await step.Execute(context);
                if (!result.IsSuccess) {
                    throw new CommandStepExecutionException(identity, result.Error);
                }
            } catch (Exception exception) {
                throw new CommandStepExecutionException(identity, exception.Message);
            }

            var nextIdentity = new CommandStepIdentity(identity.CommandCode, identity.Id + 1);

            await userStates.SetActiveStep(chatId, userId, nextIdentity);
            await dataStorage.SetData(chat.Id, user.Id, data);
        }
    }
}
