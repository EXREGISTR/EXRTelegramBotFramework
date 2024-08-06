using Telegram.Bot.Types;

namespace TelegramBotFramework.Extensions {
    public static class UserExtensions {
        public static string GetUsername(this User user) {
            return user.Username != null
                ? $"@{user.Username}"
                : $"{user.FirstName} {user.LastName}";
        }
    }
}
