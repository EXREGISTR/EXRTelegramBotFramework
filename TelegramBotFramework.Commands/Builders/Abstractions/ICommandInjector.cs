using Microsoft.Extensions.DependencyInjection;

namespace TelegramBotFramework.Commands.Builders {
    internal interface ICommandInjector {
        public void Inject(IServiceCollection services);
    }
}
