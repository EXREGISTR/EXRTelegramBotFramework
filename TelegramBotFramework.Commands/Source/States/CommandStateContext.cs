using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotFramework.Commands.States {
    public record CommandStateContext(
        Chat Chat,
        User Sender,
        Message Message,
        ITelegramBotClient Bot);

    public record CommandStateContext<TData>(
        Chat Chat,
        User Sender,
        Message Message,
        TData Data,
        ITelegramBotClient Bot);
}
