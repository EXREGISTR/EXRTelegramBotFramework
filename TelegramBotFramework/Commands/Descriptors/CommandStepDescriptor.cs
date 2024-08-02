using TelegramBotFramework.Commands.Proxies.Contracts;

namespace TelegramBotFramework.Commands.Descriptors {
    internal record CommandStepDescriptor(
        string CommandCode,
        int Id,
        ICommandStepProxy Proxy);
}
