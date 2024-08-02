using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using TelegramBotFramework.Commands.Contexts;
using TelegramBotFramework.Commands.Proxies.Contracts;
using TelegramBotFramework.Commands.Results;

namespace TelegramBotFramework.Commands.Proxies {
    internal class CommandProxy(
        string commandCode, 
        IServiceProvider services) : ICommandProxy {
        public Task<CommandResult> Execute(Chat chat, User sender) {
            var command = services.GetRequiredKeyedService<ICommand>(commandCode);
            var context = new CommandContext(chat, sender);

            return command.Execute(context);
        }
    }
}
