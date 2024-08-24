namespace TelegramBotFramework.Commands.Utils.Mappers
{
    internal class MapperConfiguration(
        Dictionary<Type, TypeMappingDescriptor> descriptors)
    {

        public TypeMappingDescriptor GetTypeMapping(Type targetType)
            => descriptors[targetType];
    }
}
