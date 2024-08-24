using Microsoft.Extensions.DependencyInjection;

namespace TelegramBotFramework.Commands.Builders {
    public class CommandsBuilder {
        private readonly List<ICommandInjector> injectors;

        public CommandBuilder<TCommand> AddCommand<TCommand>(
            Func<IServiceProvider, object?, TCommand>? factory = null,
            Action<CommandDescriptorBuilder>? configurator = null) 
            where TCommand : ICommand {
            var type = typeof(TCommand);
            ValidateCommandType(type);

            var descriptorBuilder = new CommandDescriptorBuilder(type, CommandType.Common);
            configurator?.Invoke(descriptorBuilder);
            var descriptor = descriptorBuilder.Build();

            var builder = new CommandBuilder<TCommand>(descriptor, factory);
            injectors.Add(builder);
            return builder;
        }

        public 

        internal void Inject(IServiceCollection services) {
            foreach (var injector in injectors) {
                injector.Inject(services);
            }

            injectors.Clear();
        }

        private void ValidateCommandType(Type type) {
            throw new NotImplementedException();
        }
    }
}
