using System.Diagnostics.CodeAnalysis;

namespace TelegramBotFramework.Commands.Temp.Builders
{
    internal class CommandEntryComparer : IEqualityComparer<CommandEntry>
    {
        public bool Equals(CommandEntry? x, CommandEntry? y)
        {
            if (x == null || y == null) return false;
            if (x.Code == y.Code) return true;

            return false;
        }

        public int GetHashCode([DisallowNull] CommandEntry obj) => obj.Code.GetHashCode();
    }
}
