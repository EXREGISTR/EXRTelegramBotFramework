using Telegram.Bot.Types;

namespace TelegramBotFramework.Commands.Processors {
    internal class HighPriorityCommandsProcessor : UpdateProcessor<Message> {
        protected override Task Process(Message data, IServiceProvider services, IUpdateProcessor<Message> next) {

        }
    }



    // var builder = services.AddCommands();
    // builder
    //     .AddStatelessCommand<CreateWarriorCommand>() Это будет конечный обработчик, когда все стейты выполнил
    //     .Parameterize<WarriorCreationContext>() создается билдер который заставляет регать стейты IState<TData>
    //     .AddState<>
    // 
}
