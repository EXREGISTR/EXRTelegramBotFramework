using Telegram.Bot.Types;

namespace TelegramBotFramework.Commands.Contexts {
    public record CommandContext(Chat Chat, User Sender);
    public record CommandContext<TData>(Chat Chat, User Sender, TData Data);
}
