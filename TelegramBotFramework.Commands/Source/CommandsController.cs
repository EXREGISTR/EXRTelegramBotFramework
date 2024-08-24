using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotFramework.Commands.Exceptions;
using TelegramBotFramework.Commands.Extensions;
using TelegramBotFramework.Commands.Utils.Parsers.Contracts;

namespace TelegramBotFramework.Commands {
    internal class CommandsController(
        ICommandParser parser,
        ITelegramBotClient bot,
        IServiceProvider services) {
        public Task Execute(Message message) {
            var chatType = message.Chat.Type;
            var parsingResult = parser.Parse(message.Text!, chatType);
            var code = parsingResult.Code;

            var arguments = parsingResult.Arguments;

            var descriptor = services.GetDescriptor(code)
                ?? throw new CommandCannotBeProcessedException(code, $"Command doesn't exists");

            if (!descriptor.AvailableChatTypes.Contains(chatType)) {
                throw new CommandCannotBeProcessedException(code, $"Сommand is not intended for the chat type {chatType}");
            }

            return descriptor.Type switch {
                CommandType.Common => HandleCommand(message, code),
                CommandType.Parameterized => HandleParameterizedCommand(message, code, arguments),
                CommandType.Stateless => HandleStatelessCommand(message, code),
                _ => throw new Exception("Unknown command type"),
            };
        }

        public Task ExecuteState(Message message, CommandStateIdentity identity) {
            var proxy = services.GetState(identity);
            return proxy.Execute(chat, sender, message);
        }

        private Task HandleCommand(Message message, string code) {
            var proxy = services.GetCommand(code);
            return proxy.Execute(message.Chat, message.From!);
        }

        private Task HandleParameterizedCommand(Message message, string code, string[] arguments) {
            try {
                var proxy = services.GetParameterizedCommand(code);
                return proxy.Execute(message.Chat, message.From!, arguments);
            } catch (CommandSignatureException exception) {
                return bot.SendTextMessageAsync(message.Chat.Id, exception.Message, replyToMessageId: message.MessageId);
            }
        }

        private Task HandleStatelessCommand(Message message, string code) {
            var proxy = services.GetInitialState();
            return proxy.Execute(message.Chat, message.From!, message);
        }
    }
}
