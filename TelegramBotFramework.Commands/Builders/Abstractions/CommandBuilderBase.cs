using Microsoft.Extensions.DependencyInjection;

namespace TelegramBotFramework.Commands.Builders {
    public abstract class CommandBuilderBase : ICommandInjector {
        internal readonly CommandDescriptor descriptor;

        internal CommandBuilderBase(CommandDescriptor descriptor) {
            this.descriptor = descriptor;
        }

        void ICommandInjector.Inject(IServiceCollection services) {
            services.AddSingleton(descriptor);
            Inject(services);
        }

        protected abstract void Inject(IServiceCollection services);
    }
}
