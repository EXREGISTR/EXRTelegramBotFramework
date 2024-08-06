using Telegram.Bot.Types;

namespace TelegramBotFramework.Commands.Proxies.Contracts {
    internal interface ICommandProxy {
        public Task Execute(Chat chat, User user);
    }
}
