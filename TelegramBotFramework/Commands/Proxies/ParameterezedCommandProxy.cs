using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using TelegramBotFramework.Commands.Contexts;
using TelegramBotFramework.Commands.Parsers.Contracts;
using TelegramBotFramework.Commands.Proxies.Contracts;
using TelegramBotFramework.Commands.Results;

namespace TelegramBotFramework.Commands.Proxies {
    internal class ParameterezedCommandProxy<TData>(
        string commandCode,
        string[] argumentNames,
        ICommandDataParser parser,
        IServiceProvider services)
        : IParameterizedCommandProxy where TData : class {
        public Task<CommandResult> Execute(Chat chat, User user, string[] arguments) {
            if (arguments.Length != argumentNames.Length) {
                return Task.FromResult(CommandResult.Failure("Not enough parameters"));
            }

            var data = parser.Parse<TData>(argumentNames, arguments);
            var context = new CommandContext<TData>(chat, user, data);

            var command = services.GetRequiredKeyedService<ICommand<TData>>(commandCode);
            return command.Execute(context);
        }
    }
}
