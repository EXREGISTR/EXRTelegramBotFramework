using Telegram.Bot.Types;

namespace TelegramBotFramework {
    internal delegate Task UpdateProcessor(IServiceProvider services, Update update);
    internal delegate Task MessageProcessor(IServiceProvider services, Message message);
}
