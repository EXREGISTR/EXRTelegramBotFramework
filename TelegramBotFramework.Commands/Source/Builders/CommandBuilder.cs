namespace TelegramBotFramework.Commands.Builders {
    public class CommandBuilder {
        private readonly CommandEntry data;

        internal CommandBuilder(CommandEntry data) {
            this.data = data;
        }

        public CommandBuilder WithDescription(string description) {
            if (string.IsNullOrWhiteSpace(description)) {
                throw new InvalidDataException("Invalid description");
            }

            if (description.Length > CommandEntry.MaxDescriptionLength) {
                throw new InvalidDataException("Invalid description");
            }

            data.Description = description;
            return this;
        }

        public CommandBuilder WithHelp(string helpText) {
            if (string.IsNullOrWhiteSpace(helpText)) {
                throw new InvalidDataException("Invalid help text");
            }

            if (helpText.Length > CommandEntry.MaxHelpLength) {
                throw new InvalidDataException("Invalid help text");
            }

            data.Description = helpText;
            return this;
        }
    }
}
