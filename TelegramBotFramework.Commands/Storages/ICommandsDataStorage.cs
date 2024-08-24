namespace TelegramBotFramework.Commands.Storages
{
    internal interface ICommandsDataStorage
    {
        public Task<TData?> GetData<TData>(string key) where TData : class;
        public Task UpdateData<TData>(string key, TData data) where TData : class;
        public Task DeleteData(string key);
    }
}