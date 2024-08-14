namespace TelegramBotFramework.Commands {
    public record CommandStepIdentity {
        public string CommandCode { get; init; }
        public int Index { get; init; }

        private CommandStepIdentity(string commandCode, int index) {
            CommandCode = commandCode;
            Index = index;
        }

        public static CommandStepIdentity First(string commandCode) => new(commandCode, 0);

        public static CommandStepIdentity Create(string commandCode, int position) 
            => new(commandCode, position);

        public static CommandStepIdentity Next(CommandStepIdentity identity) 
            => new(identity.CommandCode, identity.Index + 1);
    }
}
