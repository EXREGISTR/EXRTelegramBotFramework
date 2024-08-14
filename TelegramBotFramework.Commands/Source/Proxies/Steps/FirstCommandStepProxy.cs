using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using TelegramBotFramework.Commands.Contexts;
using TelegramBotFramework.Commands.Proxies.Contracts;
using TelegramBotFramework.Commands.Storages.Contracts;
using TelegramBotFramework.Exceptions;

namespace TelegramBotFramework.Commands.Proxies.Steps {
    internal class FirstCommandStepProxy<TData>(
        CommandStepIdentity identity,
        ICommandStepsDataStorage dataStorage,
        IUserCommandStatesStorage userStates,
        IServiceProvider services)
        : ICommandStepProxy where TData : class {
        public async Task Execute(Chat chat, User user, Message message) {
            var data = services.GetService<TData>();
            data ??= Activator.CreateInstance<TData>();

            var chatId = chat.Id;
            var userId = user.Id;

            var context = new CommandStepContext<TData>(chat, user, message, data);
            var command = services.GetRequiredKeyedService<ICommandStep<TData>>(identity);

            await userStates.SetActiveStep(chatId, userId, identity).ConfigureAwait(false);

            try {
                var result = await command.Execute(context);
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
