using TelegramBotFramework.Commands.Parsers;
using Xunit;

namespace TelegramBotFramework.Tests {
    public class CommmandDataParserTests {
        public class CreateWarriorData {
            public string Nickname { get; set; } = string.Empty;
            public string Smile { get; set; } = string.Empty;
        }

        [Fact]
        public void DataShouldBeInitialized() {
            string[] argumentNames = ["nickname", "smile"];
            string[] arguments = ["EXREGISTR", ":)"];

            var parser = new DefaultCommandDataParser();
            var data = parser.Parse<CreateWarriorData>(argumentNames, arguments);

            Assert.NotNull(data);
            Assert.NotEmpty(data.Nickname);
            Assert.NotEmpty(data.Smile);
        }
    }
}
