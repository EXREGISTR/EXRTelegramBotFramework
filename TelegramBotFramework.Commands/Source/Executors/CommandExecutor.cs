using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotFramework.Commands.Executors {
    internal sealed class CommandExecutor(ICommand command, ITelegramBotClient bot) {
        public Task Execute(Chat chat, User user) {
            var context = new CommandContext(chat, user, bot);
            return command.Execute(context);
        }
    }
}
