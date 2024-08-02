using System.Reflection;
using TelegramBotFramework.Commands.Parsers.Contracts;

namespace TelegramBotFramework.Commands.Parsers {
    internal class DefaultCommandDataParser : ICommandDataParser {
        public TData Parse<TData>(string[] argumentNames, string[] arguments) where TData : class {
            if (argumentNames.Length != arguments.Length) {
                throw new ArgumentException("The number of argument names must match the number of arguments.");
            }

            var data = Activator.CreateInstance<TData>();

            var properties = typeof(TData).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var length = argumentNames.Length;
            for (int i = 0; i < length; i++) {
                string argumentName = argumentNames[i];
                string argument = arguments[i];

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
