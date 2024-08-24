using Microsoft.Extensions.DependencyInjection;
using TelegramBotFramework.Commands.Options;
using TelegramBotFramework.Commands.Temp.Builders;
using TelegramBotFramework.Commands.Utils.Parsers;
using TelegramBotFramework.Commands.Utils.Parsers.Contracts;

namespace TelegramBotFramework.Commands
{
    public static class CommandsInjection {
        public static CommandsBuilderOld AddCommands(
            this IServiceCollection services, Action<CommandsSettings> configurator) {
            var settings = new CommandsSettings(services);
            configurator(settings);

            services.AddSingleton<ICommandParser, DefaultCommandParser>();
            services.AddSingleton<ICommandDataParser, DefaultCommandDataParser>();

            return new CommandsBuilderOld(services);
        }
    }
}
