namespace TelegramBotFramework.Commands.Parsers.Contracts {
    public interface ICommandDataParser {
        public TData Parse<TData>(string[] argumentNames, string[] arguments) where TData : class;
    }
}
