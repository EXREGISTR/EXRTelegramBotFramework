using Telegram.Bot.Types;

namespace TelegramBotFramework.Commands.Contexts {
    public record CommandStepContext<TData>(Chat Chat, User Sender, Message Message, TData Context);
}
