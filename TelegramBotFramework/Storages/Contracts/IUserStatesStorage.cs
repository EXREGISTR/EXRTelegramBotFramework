using TelegramBotFramework.Commands;

namespace TelegramBotFramework.Storages.Contracts {
    public interface IUserStatesStorage {
        public Task<CommandStepIdentity?> GetActiveStep(long chatId, long userId);
        public Task SetActiveStep(long chatId, long userId, CommandStepIdentity identity);
        public Task DeleteActiveStep(long chatId, long userId);
    }
}
