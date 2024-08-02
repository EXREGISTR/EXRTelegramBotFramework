using Telegram.Bot.Types;
using TelegramBotFramework.Commands.Results;

namespace TelegramBotFramework.Commands.Proxies.Contracts {
    internal interface IParameterizedCommandProxy {
        public Task<CommandResult> Execute(Chat chat, User user, string[] arguments);
    }
}
