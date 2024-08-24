using System.Text.RegularExpressions;

namespace TelegramBotFramework.Commands.Utils {
    internal partial class CommandCodeCreator {
        public static string Create(Type commandType) {
            var typeName = commandType.Name;
            var match = TakeNameExcludeCommand().Match(typeName);

            if (match.Success) {
                // all parts exclude "Command"
                string command = match.Groups[1].Value;
                return ConvertToSnakeCase(command);
            }

            return ConvertToSnakeCase(typeName);
        }

        [GeneratedRegex(@"^(.*?)Command", RegexOptions.IgnoreCase)]
        private static partial Regex TakeNameExcludeCommand();

        private static string ConvertToSnakeCase(string input)
            => ConvertingToSnakeCase().Replace(input, "_$1").TrimStart('_').ToLower();

        [GeneratedRegex(@"([A-Z])")]
        private static partial Regex ConvertingToSnakeCase();
    }
}
