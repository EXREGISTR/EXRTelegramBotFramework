using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotFramework.Commands {
    public record CommandContext<TData>(Chat Chat, User Sender, TData Data, ITelegramBotClient Bot);
    public record CommandContext(Chat Chat, User Sender, ITelegramBotClient Bot);
}
