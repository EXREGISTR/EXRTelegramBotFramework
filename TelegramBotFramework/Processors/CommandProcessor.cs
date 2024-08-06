using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotFramework.Commands;
using TelegramBotFramework.Commands.Descriptors;
using TelegramBotFramework.Commands.Parsers.Contracts;
using TelegramBotFramework.Commands.Proxies.Contracts;
using TelegramBotFramework.Exceptions;

namespace TelegramBotFramework.Processors {
    internal class CommandProcessor(
        IServiceProvider services,
        ICommandParser parser, 
        ITelegramBotClient bot) {
        public Task Process(Message message) {
            var chatType = message.Chat.Type;
            var parsingResult = parser.Parse(message.Text!, chatType);
            var code = parsingResult.Code;

            var arguments = parsingResult.Arguments;

            var descriptor = services.GetKeyedService<CommandDescriptor>(code)
                ?? throw new CommandCannotBeProcessedException(code, $"Command doesn't exists");

            if (!descriptor.AvailableChatTypes.Contains(chatType)) {
                throw new CommandCannotBeProcessedException(code, $"Сommand is not intended for the chat type {chatType}");
            }

            return descriptor.Type switch {
                CommandType.Common => HandleCommand(message, code),
                CommandType.Parameterized => HandleParameterizedCommand(message, code, arguments),
                CommandType.StepByStep => HandleStepByStepCommand(message, code),
                _ => throw new Exception("Unknown command type"),
            };
        }

        public Task Process(Message message, CommandStepIdentity stepId) {
            var proxy = services.GetRequiredKeyedService<ICommandStepProxy>(stepId);
            return proxy.Execute(message.Chat, message.From!, message);
        }

        private Task HandleCommand(Message message, string code) {
            var proxy = services.GetRequiredKeyedService<ICommandProxy>(code);
            return proxy.Execute(message.Chat, message.From!);
        }

        private Task HandleParameterizedCommand(Message message, string code, string[] arguments) {
            try {
                var proxy = services.GetRequiredKeyedService<IParameterizedCommandProxy>(code);
                return proxy.Execute(message.Chat, message.From!, arguments);
            } catch (CommandSignatureException exception) {
                return bot.SendTextMessageAsync(message.Chat.Id, exception.Message, replyToMessageId: message.MessageId);
            }
        }

        private Task HandleStepByStepCommand(Message message, string code) {
            var firstStepId = new CommandStepIdentity(code, 0);

            var proxy = services.GetRequiredKeyedService<ICommandStepProxy>(firstStepId);
            return proxy.Execute(message.Chat, message.From!, message);
        }
    }
}
