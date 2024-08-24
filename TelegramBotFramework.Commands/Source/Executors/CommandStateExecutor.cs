using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotFramework.Commands.States;
using TelegramBotFramework.Commands.Storages;

namespace TelegramBotFramework.Commands.Executors {
    internal sealed class CommandStateExecutor(
        CommandStateIdentity identity,
        IServiceProvider services)
        : ICommandStateExecutor {
        private readonly CommandStateIdentitiesStorage statesStorage
            = services.GetRequiredService<CommandStateIdentitiesStorage>();

        private readonly ICommandState state
            = services.GetRequiredKeyedService<ICommandState>(identity);

        private readonly StateChanger stateChanger = new();

        public async Task Execute(Chat chat, User user, Message message) {
            var chatId = chat.Id;
            var userId = user.Id;

            var bot = services.GetRequiredService<ITelegramBotClient>();
            var context = new CommandStateContext(chat, user, message, bot);


            await state.Execute(context, stateChanger);

            if (stateChanger.NeedToEndHandling) {
                await statesStorage.Delete(chatId, userId);
                return;
            }

            var nextIdentity = stateChanger.NextIdentity;
            if (nextIdentity != null) {
                await statesStorage.Update(chatId, userId, nextIdentity);
                var preNextState = services.GetKeyedService<IBeforeEnteringStateHandler>(nextIdentity);
                if (preNextState != null) {
                    await preNextState.BeforeEnter(context);
                }
            }
        }
    }
}
