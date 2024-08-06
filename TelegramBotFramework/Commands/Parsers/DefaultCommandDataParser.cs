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

            var data = Activator.CreateInstance<TData>();

            var properties = typeof(TData).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var length = argumentNames.Length;
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
