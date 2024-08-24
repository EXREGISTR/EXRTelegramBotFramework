using System.Reflection;
using TelegramBotFramework.Commands.Exceptions;

namespace TelegramBotFramework.Commands.Utils.Mappers {
    internal record WarriorContext(string Name, int Age);

    internal class Mapper(MapperConfiguration configuration) {
        // arguments: "EXREGISTR", 5
        public object Map(Type targetType, string[] arguments) {
            var descriptor = configuration.GetTypeMapping(targetType);
            var constructor = descriptor.Constructor;

            if (constructor == null) {
                return MapByProperties(descriptor, arguments);
            }

            if (descriptor.PropertyMappings.Length != arguments.Length) {
                throw new CommandSignatureException("Неверно переданы аргументы!");
            }

            return MapByConstructor(descriptor, constructor, arguments);
        }

        private static object MapByConstructor(
            TypeMappingDescriptor descriptor,
            ConstructorInfo constructor,
            string[] inputArguments) {
            var parameters = constructor.GetParameters();

            var argumentsForConstructor = new object?[parameters.Length];

            for (var index = 0; index < parameters.Length; index++) {
                var input = inputArguments[index];
                var parameter = parameters[index];
                var propertyDescriptor = descriptor.PropertyMappings[index];

                var value = propertyDescriptor.Mapper(input);
                argumentsForConstructor[index] = value;
            }

            return constructor.Invoke(argumentsForConstructor);
        }

        private static object MapByProperties(TypeMappingDescriptor descriptor, string[] inputArguments) {
            var type = descriptor.TargetType;
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var instance = Activator.CreateInstance(type)
                ?? throw new InvalidOperationException(
                    $"Something went wrong while creating instance of type {type.Name}");

            for (var index = 0; index < properties.Length; index++) {
                var input = inputArguments[index];
                var property = properties[index];
                var propertyDescriptor = descriptor.PropertyMappings[index];

                var value = propertyDescriptor.Mapper(input);
                property.SetValue(instance, value);
            }

            return instance;
        }
    }
}
