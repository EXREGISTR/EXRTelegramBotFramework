using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;
using TelegramBotFramework.Commands;
using TelegramBotFramework.Commands.Builders;
using TelegramBotFramework.Commands.Contexts;
using TelegramBotFramework.Exceptions;
using Xunit;

namespace TelegramBotFramework.Tests {
    public class CreateWarriorCommandWithoutParameters : ICommand {
        public Task Execute(CommandContext context) {
            throw new NotImplementedException();
        }
    }

    public record CreateWarriorContext(string Name, string Smile);

    public class CreateWarrior : ICommand<CreateWarriorContext> {
        public Task Execute(CommandContext<CreateWarriorContext> context) {
            throw new NotImplementedException();
        }
    }

    public class CommandsBuilderTests {
        [Fact]
        public void Test() {
            var services = new ServiceCollection();
            var builder = new CommandsBuilder(services);

            builder.AddParameterizedCommand<CreateWarrior, CreateWarriorContext>(
                "create_warrior name smile",
                [ChatType.Supergroup, ChatType.Group]);
        }
    }
}
