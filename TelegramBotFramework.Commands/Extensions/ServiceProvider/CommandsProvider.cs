using Microsoft.Extensions.DependencyInjection;

namespace TelegramBotFramework.Commands.Extensions {
    internal static class CommandsProvider {
        public static CommandDescriptor? GetDescriptor(
            this IServiceProvider services, string code) 
            => services.GetRequiredKeyedService<CommandDescriptor>(code);
    }
}
