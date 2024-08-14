using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBotFramework.Extensions {

    public static class ChatExtensions {
        public static string GetTitle(this Chat chat) {
            return chat.Type switch {
                ChatType.Group => chat.GetGroupOrChannelTitle(),
                ChatType.Private => GetPrivateChatName(),
                ChatType.Sender => GetPrivateChatName(),
                ChatType.Channel => chat.GetGroupOrChannelTitle(),
                ChatType.Supergroup => chat.GetGroupOrChannelTitle(),
                _ => throw new Exception("Unknown chat type"),
            };
        }

        private static string GetGroupOrChannelTitle(this Chat chat) => chat.Title!;

        private static string GetPrivateChatName()
            => "private chat";
    }
}
