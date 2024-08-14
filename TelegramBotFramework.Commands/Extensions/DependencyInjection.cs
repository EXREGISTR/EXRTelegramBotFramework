using TelegramBotFramework.Commands.Builders;
using TelegramBotFramework.Commands.Parsers.Contracts;
using TelegramBotFramework.Commands.Parsers;
using TelegramBotFramework.Commands.Options;
using Microsoft.Extensions.DependencyInjection;

namespace TelegramBotFramework.Commands.Extensions {
    public static class DependencyInjection {
        public static CommandsBuilder AddCommands(
            this IServiceCollection services, Action<CommandsSettings> configurator) {
            var settings = new CommandsSettings(services);
            configurator(settings);

            services.AddSingleton<ICommandParser, DefaultCommandParser>();
            services.AddSingleton<ICommandDataParser, DefaultCommandDataParser>();

            return new CommandsBuilder(services);
        }
    }
}
