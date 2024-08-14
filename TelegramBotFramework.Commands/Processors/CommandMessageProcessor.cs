using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using TelegramBotFramework.Commands.Extensions;
using TelegramBotFramework.Commands.Storages.Contracts;
using TelegramBotFramework.UpdateProcessing;

namespace TelegramBotFramework.Commands.Processors {
    internal class CommandMessageProcessor : UpdateProcessor<Message> {
        protected override async Task Process(Message message, IServiceProvider services, IUpdateProcessor<Message> next) {
            var commandExecutor = services.GetRequiredService<CommandExecutor>();
            var userStatesStorage = services.GetRequiredService<IUserCommandStatesStorage>();

            var currentCommandStep = await userStatesStorage
                .GetActiveStep(message.Chat.Id, message.From!.Id)
                .ConfigureAwait(false);

            if (currentCommandStep != null) {
                await commandExecutor.Process(message, currentCommandStep)
                    .ConfigureAwait(false);
                return;
            }

            if (message.IsCommand()) {
                await commandExecutor.Execute(message)
                    .ConfigureAwait(false);
            } else {
                await next.Process(message, services)
                    .ConfigureAwait(false);
            }
        }
    }
}
