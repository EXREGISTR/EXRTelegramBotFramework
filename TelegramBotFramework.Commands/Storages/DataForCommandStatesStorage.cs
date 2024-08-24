namespace TelegramBotFramework.Commands.Storages {
    internal sealed class DataForCommandStatesStorage(ICommandsDataStorage storage) {
        private static string CreateKey(long chatId, long userId)
            => $"{chatId}-{userId}-datas";

        public Task<TData?> Retrieve<TData>(long chatId, long userId)
            where TData : class
            => storage.GetData<TData>(CreateKey(chatId, userId));

        public Task Update<TData>(long chatId, long userId, TData data)
            where TData : class
            => storage.UpdateData(CreateKey(chatId, userId), data);

        public Task Delete(long chatId, long userId)
            => storage.DeleteData(CreateKey(chatId, userId));
    }
}
