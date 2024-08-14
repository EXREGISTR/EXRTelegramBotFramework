using Microsoft.Extensions.DependencyInjection;
using TelegramBotFramework.Commands.Proxies.Contracts;

namespace TelegramBotFramework.Commands.Extensions {
    internal static class CommandsProvider {
        public static ICommandProxy GetCommand(
            this IServiceProvider services, string code)
            => services.GetRequiredKeyedService<ICommandProxy>(code);

        public static IParameterizedCommandProxy GetParameterizedCommand(
            this IServiceProvider services, string code)
            => services.GetRequiredKeyedService<IParameterizedCommandProxy>(code);

        public static ICommandStepProxy GetCommandStep(this IServiceProvider services, CommandStepIdentity stepId)
            => services.GetRequiredKeyedService<ICommandStepProxy>(stepId);

        public static ICommandStepProxy GetFirstCommandStep(this IServiceProvider services, string code) {
            var stepId = CommandStepIdentity.First(code);
            return services.GetCommandStep(stepId);
        }

        public static CommandDescriptor? GetCommandDescriptor(this IServiceProvider services, string code)
            => services.GetKeyedService<CommandDescriptor>(code);
    }
}
