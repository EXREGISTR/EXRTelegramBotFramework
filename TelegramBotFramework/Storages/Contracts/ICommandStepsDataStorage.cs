namespace TelegramBotFramework.Storages.Contracts {
    public interface ICommandStepsDataStorage {
        public Task<TData?> GetData<TData>(long chatId, long userId) where TData : class;
        public Task SetData<TData>(long chatId, long userId, TData data) where TData: class;
    }
}
