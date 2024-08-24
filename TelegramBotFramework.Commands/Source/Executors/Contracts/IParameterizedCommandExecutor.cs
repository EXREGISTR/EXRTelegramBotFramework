using Telegram.Bot.Types;

namespace TelegramBotFramework.Commands.Executors {
    internal interface IParameterizedCommandExecutor {
        public Task Execute(Chat chat, User user, string[] arguments);
    }
}
