using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using TelegramBotFramework.Commands.Extensions;
using TelegramBotFramework.Commands.Storages;
using TelegramBotFramework.Commands.Storages.Cached;

namespace TelegramBotFramework.Commands {
    internal class HighPriorityCommandsProcessor : UpdateProcessor<Message> {
        protected override Task Process(Message data, IServiceProvider services, IUpdateProcessor<Message> next) {

        }
    }

    internal class CommandsProcessor : UpdateProcessor<Message> {
        protected override async Task Process(Message message, IServiceProvider services, IUpdateProcessor<Message> next) {
            var dataStorage = services.GetRequiredService<CommandStateIdentitiesStorage>();
            var executor = services.GetRequiredService<CommandsController>();

            var chatId = message.Chat.Id;
            var senderId = message.From!.Id;

            var identity = await dataStorage.Retrieve(chatId, senderId);
            if (identity != null) {
                await executor
                    .ExecuteState(message, identity)
                    .ConfigureAwait(false);
                return;
            }

            if (message.IsCommand()) {
                await executor
                    .Execute(message)
                    .ConfigureAwait(false);
            } else {
                await next
                    .Process(message, services)
                    .ConfigureAwait(false);
            }
        }
    }



    // var builder = services.AddCommands();
    // builder
    //     .AddStatelessCommand<CreateWarriorCommand>() Это будет конечный обработчик, когда все стейты выполнил
    //     .Parameterize<WarriorCreationContext>() создается билдер который заставляет регать стейты IState<TData>
    //     .AddState<>
    // 
}
