using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotFramework.Commands.Exceptions;
using TelegramBotFramework.Commands.States;
using TelegramBotFramework.Commands.Storages;

namespace TelegramBotFramework.Commands.Executors {
    // регать на command code и имя типа стейта
    internal sealed class ParameterizedCommandStateExecutor<TData>(
        string commandCode,
        CommandStateIdentity identity,
        IServiceProvider services)
        : ICommandStateExecutor where TData : class {
        private readonly DataForCommandStatesStorage dataStorage
            = services.GetRequiredService<DataForCommandStatesStorage>();

        private readonly CommandStateIdentitiesStorage statesStorage
            = services.GetRequiredService<CommandStateIdentitiesStorage>();

        private readonly ICommandState<TData> state
            = services.GetRequiredKeyedService<ICommandState<TData>>(identity);

        private readonly StateChanger<TData> stateChanger = new();

        public async Task Execute(Chat chat, User sender, Message message) {
            var chatId = chat.Id;
            var userId = sender.Id;

            var data = await dataStorage.Retrieve<TData>(chatId, userId);
            data ??= services.GetRequiredService<TData>();

            var bot = services.GetRequiredService<ITelegramBotClient>();
            var context = new CommandStateContext<TData>(chat, sender, message, data, bot);

            try {
                await state.Execute(context, stateChanger);
            } catch (Exception exception) {
                throw new CommandStateExecutionException(identity, exception.Message);
            }

            if (stateChanger.FinishWorkWithUser) {
                await FinishWorkingWithUser(chat, sender, data, bot);
                return;
            }

            var nextIdentity = stateChanger.NextIdentity;
            if (nextIdentity != null) {
                await HandleTransitToNextState(chatId, userId, context, nextIdentity);
            }

            await dataStorage.Update(chatId, userId, data);
        }

        private async Task HandleTransitToNextState(
            long chatId, long userId,
            CommandStateContext<TData> context,
            CommandStateIdentity nextIdentity) {
            await statesStorage.Update(chatId, userId, nextIdentity);
            var preNextState = services.GetKeyedService<IBeforeEnteringStateHandler<TData>>(nextIdentity);
            if (preNextState != null) {
                await preNextState.BeforeEnter(context);
            }
        }

        private async Task FinishWorkingWithUser(Chat chat, User sender, TData data, ITelegramBotClient bot) {
            var chatId = chat.Id;
            var senderId = sender.Id;

            var onFinishStatesHandler = services.GetKeyedService<ICommand<TData>>(commandCode);
            if (onFinishStatesHandler != null) {
                var commandContext = new CommandContext<TData>(chat, sender, data, bot);
                await onFinishStatesHandler.Execute(commandContext);
            }

            await statesStorage.Delete(chatId, senderId);
            await dataStorage.Delete(chatId, senderId);
        }
    }
}
