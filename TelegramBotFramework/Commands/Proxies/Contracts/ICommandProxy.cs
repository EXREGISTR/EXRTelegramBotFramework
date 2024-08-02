using Telegram.Bot.Types;
using TelegramBotFramework.Commands.Results;

namespace TelegramBotFramework.Commands.Proxies.Contracts {
    internal interface ICommandProxy {
        public Task<CommandResult> Execute(Chat chat, User user);
    }
}
