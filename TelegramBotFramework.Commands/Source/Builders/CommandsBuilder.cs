using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Telegram.Bot.Types.Enums;
using TelegramBotFramework.Commands.Parsers.Contracts;
using TelegramBotFramework.Commands.Proxies;
using TelegramBotFramework.Commands.Proxies.Contracts;
using TelegramBotFramework.Exceptions;

namespace TelegramBotFramework.Commands.Builders {
    public class CommandsBuilder(IServiceCollection services) {
        private readonly HashSet<CommandEntry> commands = new(new CommandEntryComparer());

        public CommandBuilder AddCommand<TCommand>(
            string inputCommandCode, 
            IEnumerable<ChatType> availableChatTypes, 
            Func<IServiceProvider, object?, TCommand>? factory = null) 
            where TCommand : class, ICommand {
            var code = ConvertAndValidateCommandCode(inputCommandCode);
            var data = CreateEntry(code, CommandType.Common, availableChatTypes);
            RegisterCommand(code, factory);

            return new CommandBuilder(data);
        }

        public CommandBuilder AddParameterizedCommand<TCommand, TData>(
            string inputCommandCode, 
            IEnumerable<ChatType> availableChatTypes, 
            Func<IServiceProvider, object?, TCommand>? factory = null) 
            where TCommand : class, ICommand<TData> 
            where TData : class {
            var (code, parameters) = ParseParameterizedCommandCode(inputCommandCode, typeof(TData));
            var data = CreateEntry(code, CommandType.Parameterized, availableChatTypes);
            RegisterParameterizedCommand<TCommand, TData>(code, parameters, factory);

            return new CommandBuilder(data);
        }

        public StepByStepCommandBuilder<TData> AddStepByStepCommand<TCommand, TData>(
            string inputCommandCode, 
            IEnumerable<ChatType> availableChatTypes,
            Func<IServiceProvider, object?, TCommand>? factory)
            where TCommand : class, ICommand<TData>
            where TData : class {
            var code = ConvertAndValidateCommandCode(inputCommandCode);
            var data = CreateEntry(code, CommandType.StepByStep, availableChatTypes);

            if (factory == null) {
                services.AddKeyedScoped<ICommand<TData>, TCommand>(code);
            } else {
                services.AddKeyedScoped<ICommand<TData>, TCommand>(code, factory);
            }

            return new StepByStepCommandBuilder<TData>(code, data, services);
        } 

        private CommandEntry CreateEntry(string commandCode, CommandType type, IEnumerable<ChatType> availableChatTypes) {
            var data = new CommandEntry(commandCode, type, availableChatTypes);
            var result = commands.Add(data);

            if (!result) {
                throw new InvalidOperationException($"Command with code {commandCode} already exists!");
            }

            return data;
        }

        private void RegisterCommand<TCommand>(
            string commandCode, 
            Func<IServiceProvider, object?, TCommand>? factory) where TCommand : class, ICommand {
            if (factory == null) {
                services.AddKeyedScoped<ICommand, TCommand>(commandCode);
            } else {
                services.AddKeyedScoped<ICommand, TCommand>(commandCode, factory);
            }

            services.AddKeyedScoped<ICommandProxy, CommandProxy>(
                commandCode,
                (services, key) => {
                    return new CommandProxy(commandCode, services);
                });
        }

        private void RegisterParameterizedCommand<TCommand, TData>(
            string commandCode, 
            string[] parameters,
            Func<IServiceProvider, object?, TCommand>? factory) 
            where TCommand : class, ICommand<TData>
            where TData : class {
            if (factory == null) {
                services.AddKeyedScoped<ICommand<TData>, TCommand>(commandCode);
            } else {
                services.AddKeyedScoped<ICommand<TData>, TCommand>(commandCode, factory);
            }

            
            services.AddKeyedScoped<IParameterizedCommandProxy>(
                commandCode,
                (services, key) => {
                    var parser = services.GetRequiredService<ICommandDataParser>();
                    return new ParameterezedCommandProxy<TData>(commandCode, parameters, parser, services);
                });
        }

        private static string ConvertAndValidateCommandCode(string input) {
            var parts = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) {
                throw new CommandSignatureException(
                    "Invalid command code!\n" +
                    $"You entered: {input}");
            }

            if (parts.Length > 1) {
                throw new CommandSignatureException(
                    "The command code without parameters must consist of one word!\n" +
                    $"You entered: {input}");
            }

            string code = parts[0].TrimStart('/');
            return code;
        }

        private static (string code, string[] parameters) ParseParameterizedCommandCode(
            string input, Type dataType) {
            var parts = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0) {
                throw new CommandSignatureException(
                    "Invalid command code!\n" +
                    $"You entered: {input}");
            }

            if (parts.Length == 1) {
                throw new CommandSignatureException(
                    "Invalid command code!\n" +
                    $"You entered: {input}");
            }

            string code = parts[0].TrimStart('/');
            string[] parameters = parts.Skip(1).ToArray();

            var properties = dataType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var parameter in parameters) {
                var founded = properties.FirstOrDefault(
                    p => p.Name.Equals(parameter, StringComparison.OrdinalIgnoreCase)) 
                    ?? throw new CommandSignatureException($"Property with name {parameter} not found!");
            }

            return (code, parameters);
        }

        internal void Build() {
            foreach (var command in commands) {
                var descriptor = new CommandDescriptor(command.Code, command.Description, command.Type, command.AvailableChatTypes);
                services.AddKeyedSingleton(command.Code, descriptor);
            }

            commands.Clear();
        }
    }
}
