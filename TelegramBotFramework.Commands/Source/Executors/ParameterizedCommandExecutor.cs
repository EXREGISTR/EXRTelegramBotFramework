using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotFramework.Commands.Utils.Mappers;

namespace TelegramBotFramework.Commands.Executors {
    internal sealed class ParameterizedCommandExecutor<TData>(
        string commandCode,
        IServiceProvider services) 
        : IParameterizedCommandExecutor {
        private readonly Mapper mapper = services.GetRequiredService<Mapper>();
        private readonly ICommand<TData> command
            = services.GetRequiredKeyedService<ICommand<TData>>(commandCode);

        public Task Execute(Chat chat, User user, string[] arguments) {
            var data = (TData)mapper.Map(typeof(TData), arguments);
            var bot = services.GetRequiredService<ITelegramBotClient>();
            var context = new CommandContext<TData>(chat, user, data, bot);

            return command.Execute(context);
        }
    }
}
