using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotFramework.Commands.Executors {
    internal sealed class CommandExecutor(string commandCode, IServiceProvider services) {
        private readonly ICommand command = services.GetRequiredKeyedService<ICommand>(commandCode);

        public Task Execute(Chat chat, User user) {
            var bot = services.GetRequiredService<ITelegramBotClient>();
            var context = new CommandContext(chat, user, bot);
            return command.Execute(context);
        }
    }
}
