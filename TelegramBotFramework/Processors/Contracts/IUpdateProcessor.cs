namespace TelegramBotFramework.Processors.Contracts {
    internal interface IUpdateProcessor<TUpdateData> {
        public Task Process(TUpdateData data);
    }
}
