using Telegram.Bot.Types;

namespace TelegramBotFramework.Commands.Proxies.Contracts {
    internal interface IParameterizedCommandProxy {
        public Task Execute(Chat chat, User user, string[] arguments);
    }
}
