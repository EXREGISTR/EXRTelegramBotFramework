using Microsoft.Extensions.DependencyInjection;
using TelegramBotFramework.Commands.Builders.Abstractions;

namespace TelegramBotFramework.Commands.Builders
{
    public class ParameterizedCommandBuilder<TCommand> : CommandBuilderBase {
        private readonly Func<IServiceProvider, object?, TCommand>? factory;

        internal ParameterizedCommandBuilder(
            CommandDescriptor descriptor,
            Func<IServiceProvider, object?, TCommand>? factory) : base(descriptor) {
            this.factory = factory;
        }

        protected override void Inject(IServiceCollection services) {
            throw new NotImplementedException();
        }
    }
}
