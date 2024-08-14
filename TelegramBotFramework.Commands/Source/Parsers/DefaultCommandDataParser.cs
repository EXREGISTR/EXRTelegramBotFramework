using System.Reflection;
using TelegramBotFramework.Commands.Parsers.Contracts;
using TelegramBotFramework.Exceptions;

namespace TelegramBotFramework.Commands.Parsers {
    internal class DefaultCommandDataParser : ICommandDataParser {
        public TData Parse<TData>(string[] argumentNames, string[] arguments) where TData : class {
            if (arguments.Length == 0) {
                throw new CommandSignatureException("Тo arguments were passed for the command that requires arguments");
            }

            if (argumentNames.Length != arguments.Length) {
                throw new CommandSignatureException("The number of argument names must match the number of arguments");
            }

            var length = argumentNames.Length;
            var dataType = typeof(TData);
            var constructor = dataType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault();
            
            if (constructor != null) {
                var parameters = constructor.GetParameters();
                var typedArguments = new object?[length];
                for (int index = 0; index < length; index++) {
                    string argument = arguments[index];
                    string argumentName = argumentNames[index];

                    var parameter = parameters.FirstOrDefault(
                        p => p.Name!.Equals(argumentName, StringComparison.OrdinalIgnoreCase))
                        ?? throw new NullReferenceException();

                    object? converted;
                    if (argument == "default") {
                        converted = Convert.ChangeType(parameter.DefaultValue, parameter.ParameterType);
                    } else {
                        converted = Convert.ChangeType(argument, parameter.ParameterType);
                    }

                    typedArguments[index] = converted;
                }

                var createdData = constructor.Invoke(typedArguments);
                return (TData)createdData; 
            }

            var data = Activator.CreateInstance<TData>();

            var properties = dataType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < length; i++) {
                string argument = arguments[i];
                if (argument == "default") continue;

                string argumentName = argumentNames[i];

                var property = properties.FirstOrDefault(
                    p => p.Name.Equals(argumentName, StringComparison.OrdinalIgnoreCase));

                if (property != null && property.CanWrite) {
                    object convertedValue = Convert.ChangeType(argument, property.PropertyType);
                    property.SetValue(data, convertedValue);
                }
            }

            return data;
        }
    }
}
