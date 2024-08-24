using Microsoft.Extensions.DependencyInjection;
using TelegramBotFramework.Commands.Executors;

namespace TelegramBotFramework.Commands.Source {
    internal static class CommandsProvider {
        public static CommandDescriptor? GetDescriptor(
            this IServiceProvider services, string code)
            => services.GetRequiredKeyedService<CommandDescriptor>(code);

        public static CommandExecutor GetCommandExecutor(
            this IServiceProvider services, string code)
            => services.GetRequiredKeyedService<CommandExecutor>(code);

        public static IParameterizedCommandExecutor GetParameterizedCommandExecutor(
            this IServiceProvider services, string code)
            => services.GetRequiredKeyedService<IParameterizedCommandExecutor>(code);

        public static ICommandStateExecutor GetInitialStateExecutor(
            this IServiceProvider services, string code)
            => services.GetRequiredKeyedService<ICommandStateExecutor>(code);

        public static ICommandStateExecutor GetStateExecutor(
            this IServiceProvider services, CommandStateIdentity identity)
            => services.GetRequiredKeyedService<ICommandStateExecutor>(identity);
    }
}
