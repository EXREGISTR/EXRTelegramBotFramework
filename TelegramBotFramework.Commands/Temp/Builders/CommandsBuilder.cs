using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;
using TelegramBotFramework.Commands.Builders;

namespace TelegramBotFramework.Commands.Temp.Builders
{
    public class CommandsBuilder(IServiceCollection services)
    {
        private readonly List<CommandBuilder> builders = [];

        public CommandBuilder<TCommand> AddCommand<TCommand>(
            Func<IServiceProvider, object?, TCommand>? factory = null)
            where TCommand : class, ICommand
        {

            var entry = new CommandDescriptorCreationData(CommandType.Common);
            var builder = new CommandBuilder(entry, typeof(TCommand));
            builders.Add(builder);
            return builder;
        }

        public ParameterizedCommandBuilder<TCommand, TData> AddParameterizedCommand<TCommand, TData>(
            Func<IServiceProvider, object?, TCommand>? factory = null)
            where TCommand : class, ICommand<TData>
            where TData : class
        {
            var entry = new CommandDescriptorCreationData(CommandType.Parameterized);
            var builder = new ParameterizedCommandBuilder<TCommand, TData>(entry);
            builders.Add(builder);
            return builder;
        }

        public StepByStepCommandBuilder<TCommand, TData> AddStepByStepCommand<TCommand, TData>()
            where TCommand : class, ICommand<TData>
            where TData : class
        {
            var entry = new CommandDescriptorCreationData(CommandType.Parameterized);
            var builder = new StepByStepCommandBuilder<TCommand, TData>(entry);
            builders.Add(builder);
            return builder;
        }

        internal void Inject()
        {
            foreach (var builder in builders)
            {
                var descriptor = builder.Build(services);
                services.AddKeyedSingleton(descriptor.Code, descriptor);
            }

            builders.Clear();
        }
    }

    public class CommandBuilder
    {
        private readonly CommandDescriptorCreationData data;
        private readonly Type commandType;

        internal CommandBuilder(CommandDescriptorCreationData data, Type commandType)
        {
            this.data = data;
            this.commandType = commandType;
        }

        internal CommandDescriptor Build()
        {
            var commandCode = data.Code ?? CommandCodeCreator.Create(commandType);
            data.AvailableChatTypes

            var availableChatTypes = data.AvailableChatTypes.Count > 0
                ? data.AvailableChatTypes
                : [ChatType.Private, ChatType.Group, ChatType.Supergroup];

            var descriptor = new CommandDescriptor(
                commandCode,
                data.Help,
                data.Type,
                availableChatTypes);

            return descriptor;
        }
    }

    public class ParameterizedCommandBuilder<TCommand, TData> : CommandBuilder
    {
        internal ParameterizedCommandBuilder(CommandDescriptorCreationData data) : base(data, typeof(TCommand)) { }


    }
}
