using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using TelegramBotFramework.Commands.Contexts;
using TelegramBotFramework.Commands.Proxies.Contracts;
using TelegramBotFramework.Exceptions;
using TelegramBotFramework.Storages.Contracts;

namespace TelegramBotFramework.Commands.Proxies.Steps {
    internal class FirstCommandStepProxy<TData>(
        CommandStepIdentity identity,
        ICommandStepsDataStorage dataStorage,
        IUserStatesStorage userStates,
        IServiceProvider services)
        : ICommandStepProxy where TData : class {
        public async Task Execute(Chat chat, User user, Message message) {
            var data = services.GetService<TData>();
            data ??= Activator.CreateInstance<TData>();

            var chatId = chat.Id;
            var userId = user.Id;

            var context = new CommandStepContext<TData>(chat, user, message, data);
            var command = services.GetRequiredKeyedService<ICommandStep<TData>>(identity);

            await userStates.SetActiveStep(chatId, userId, identity);

            try {
                var result = await command.Execute(context);
                if (!result.IsSuccess) {
                    throw new CommandStepExecutionException(identity, result.Error);
                }
            } catch (Exception exception) {
                throw new CommandStepExecutionException(identity, exception.Message);
            }

            var nextIdentity = new CommandStepIdentity(identity.CommandCode, identity.Id + 1);
            await userStates.SetActiveStep(chatId, userId, nextIdentity);
            await dataStorage.SetData(chatId, userId, data);
        }
    }
}
