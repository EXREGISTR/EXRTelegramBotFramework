using Telegram.Bot.Types;
using TelegramBotFramework.Commands;

namespace TelegramBotFramework.Processors.Contracts {
    internal interface ICommandExecutor {
        public Task Execute(Message message);
        public Task Execute(Message message, CommandStepIdentity stepId);
    }
}
