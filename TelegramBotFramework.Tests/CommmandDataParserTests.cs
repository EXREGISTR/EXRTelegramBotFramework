using TelegramBotFramework.Commands.Parsers;
using Xunit;

namespace TelegramBotFramework.Tests {
    public class CommmandDataParserTests {
        public record CreateWarriorDataRecord(string Nickname, string Smile);

        public class CreateWarriorData {
            public string Nickname { get; set; } = string.Empty;
            public string Smile { get; set; } = string.Empty;
        }

        [Fact]
        public void DataShouldBeInitialized() {
            string[] argumentNames = ["nickname", "smile"];
            string[] arguments = ["EXREGISTR", ":)"];

            var parser = new DefaultCommandDataParser();
            var data = parser.Parse<CreateWarriorDataRecord>(argumentNames, arguments);

            Assert.NotNull(data);
            Assert.NotEmpty(data.Nickname);
            Assert.NotEmpty(data.Smile);
        }
    }
}
