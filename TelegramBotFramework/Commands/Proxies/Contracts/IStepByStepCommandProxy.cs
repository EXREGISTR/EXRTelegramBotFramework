using Telegram.Bot.Types;
using TelegramBotFramework.Commands.Results;

namespace TelegramBotFramework.Commands.Proxies.Contracts {
    internal interface IStepByStepCommandProxy {
        public Task<CommandResult> Execute(Chat chat, User user);
    }
}
