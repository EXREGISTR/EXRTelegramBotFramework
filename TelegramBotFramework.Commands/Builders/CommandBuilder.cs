using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using TelegramBotFramework.Commands.Executors;

namespace TelegramBotFramework.Commands.Builders {
    public class CommandBuilder<TCommand> : CommandBuilderBase where TCommand : class, ICommand {
        private readonly Func<IServiceProvider, object?, TCommand>? factory;

        internal CommandBuilder(
            CommandDescriptor descriptor,
            Func<IServiceProvider, object?, TCommand>? factory) : base(descriptor) {
            this.factory = factory;
        }

        protected override void Inject(IServiceCollection services) {
            var commandCode = descriptor.Code;
            if (factory != null) {
                services.AddKeyedScoped<ICommand, TCommand>(commandCode, factory);
            } else {
                services.AddKeyedScoped<ICommand, TCommand>(commandCode);
            }

            services.AddKeyedScoped(
                commandCode,
                (services, key) => {
                    var command = services.GetRequiredKeyedService<ICommand>(commandCode);
                    return new CommandExecutor(command, services.GetRequiredService<ITelegramBotClient>());
                });
        }
    }
}
