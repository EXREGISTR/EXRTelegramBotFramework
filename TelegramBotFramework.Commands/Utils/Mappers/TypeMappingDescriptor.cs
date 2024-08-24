using System.Reflection;

namespace TelegramBotFramework.Commands.Utils.Mappers
{
    internal record TypeMappingDescriptor(
        Type TargetType,
        ConstructorInfo? Constructor,
        PropertyMappingDescriptor[] PropertyMappings);
}
