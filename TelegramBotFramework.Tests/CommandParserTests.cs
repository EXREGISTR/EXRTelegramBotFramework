using Xunit;
using TelegramBotFramework.Commands.Parsers;
using Telegram.Bot.Types.Enums;

namespace TelegramBotFramework.Tests {
    public class CommandParserTests {
        [Fact]
        public void CommmandCodeNotNullAndHasArgsFromPrivateChat() {
            var parser = new DefaultCommandParser("my_bot");
            var command = "/create_warrior  EXREGISTR   some_smile";

            var result = parser.Parse(command, ChatType.Private);

            Assert.All(result.Arguments, arg => Assert.False(string.IsNullOrWhiteSpace(arg)));
        }

        [Fact]
        public void CommmandCodeNotNullAndHasArgsFromGroupChat() {
            var parser = new DefaultCommandParser("my_bot");
            var command = "/create_warrior@my_bot  EXREGISTR   some_smile";

            var result = parser.Parse(command, ChatType.Group);

            Assert.All(result.Arguments, arg => Assert.False(string.IsNullOrWhiteSpace(arg)));
        }
    }
}
