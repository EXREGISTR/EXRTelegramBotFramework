namespace TelegramBotFramework.Commands.Storages {
    internal sealed class CommandStateIdentitiesStorage(ICommandsDataStorage storage) {
        private static string CreateKey(long chatId, long userId)
            => $"{chatId}-{userId}-states";

        public Task<CommandStateIdentity?> Retrieve(long chatId, long userId)
            => storage.GetData<CommandStateIdentity>(CreateKey(chatId, userId));

        public Task Update(long chatId, long userId, CommandStateIdentity identity)
            => storage.UpdateData(CreateKey(chatId, userId), identity);

        public Task Delete(long chatId, long userId)
            => storage.DeleteData(CreateKey(chatId, userId));
    }
}
