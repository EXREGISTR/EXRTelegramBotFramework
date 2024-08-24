using Telegram.Bot.Types;

namespace TelegramBotFramework.Commands.Executors {
    internal interface ICommandStateExecutor {
        public Task Execute(Chat chat, User user, Message message);
    }
}
