using Telegram.Bot.Types;

namespace TelegramBotFramework.Commands.Proxies.Contracts {
    internal interface ICommandStepProxy {
        public Task Execute(Chat chat, User user, Message message);
    }
}
