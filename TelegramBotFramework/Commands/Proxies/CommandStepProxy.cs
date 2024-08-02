using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using TelegramBotFramework.Commands.Contexts;
using TelegramBotFramework.Commands.Proxies.Contracts;
using TelegramBotFramework.Commands.Results;
using TelegramBotFramework.Storages.Contracts;

namespace TelegramBotFramework.Commands.Proxies {
    internal class CommandStepProxy<TData>(
        CommandStepIdentity identity,
        ICommandStepsDataStorage dataStorage, 
        IServiceProvider services) 
        : ICommandStepProxy where TData : class {
        public async Task<CommandResult> Execute(Chat chat, User user, Message message) {
            var data = await dataStorage.GetData<TData>(chat.Id, user.Id);
            data ??= Activator.CreateInstance<TData>();
            var context = new CommandStepContext<TData>(chat, user, message, data);
            var command = services.GetRequiredKeyedService<ICommandStep<TData>>(identity);

            var result = await command.Execute(context);
            if (result.IsSuccess) {
                await dataStorage.SetData(chat.Id, user.Id, data);
            }

            return result;
        }
    }
}
