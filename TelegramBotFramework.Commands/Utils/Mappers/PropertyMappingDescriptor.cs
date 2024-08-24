namespace TelegramBotFramework.Commands.Utils.Mappers
{
    internal record PropertyMappingDescriptor(
        Func<string, object?> Mapper);
}
