namespace EXRTelegramBotFramework.Contracts
{
    internal interface IUpdateProcessor<TUpdateData>
    {
        public Task Process(TUpdateData data, CancellationToken token);
    }
}
