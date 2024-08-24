using Telegram.Bot.Types.Enums;
using TelegramBotFramework.Commands.Utils;

namespace TelegramBotFramework.Commands.Builders {
    public sealed class CommandDescriptorBuilder {
        private readonly Type commandType;
        private readonly CommandType type;

        private List<ChatType>? availableChatTypes;
        private string? commandCode;
        private string? commandHelpText;

        internal CommandDescriptorBuilder(Type commandType, CommandType type) {
            this.commandType = commandType;
            this.type = type;
        }

        internal CommandDescriptor Build() {
            var code = commandCode ?? CommandCodeCreator.Create(commandType);
            var chatTypes = availableChatTypes ?? [ChatType.Private, ChatType.Group, ChatType.Supergroup];

            var descriptor = new CommandDescriptor(code, commandHelpText, type, chatTypes);
            return descriptor;
        }

        public CommandDescriptorBuilder WithCode(string code) {
            if (string.IsNullOrWhiteSpace(code)) {
                throw new InvalidDataException($"Invalid command code {code}");
            }

            commandCode = code;
            return this;
        }

        public CommandDescriptorBuilder WithHelp(string helpText) {
            if (string.IsNullOrWhiteSpace(helpText)) {
                throw new InvalidDataException("Invalid help text");
            }

            commandHelpText = helpText;
            return this;
        }

        public CommandDescriptorBuilder ForPrivateChat() => ForChatType(ChatType.Private);
        public CommandDescriptorBuilder ForGroup() => ForChatType(ChatType.Group);
        public CommandDescriptorBuilder ForSuperGroup() => ForChatType(ChatType.Supergroup);

        private CommandDescriptorBuilder ForChatType(ChatType chatType) {
            availableChatTypes ??= new(3);
            availableChatTypes.Add(chatType);
            return this;
        }
    }
}
