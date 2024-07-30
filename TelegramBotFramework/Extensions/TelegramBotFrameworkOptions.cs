using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using EXRTelegramBotFramework.Contracts;

namespace TelegramBotFramework
{
    public class TelegramBotFrameworkOptions(IServiceCollection services) {
        internal readonly Dictionary<UpdateType, UpdateProcessor> Processors = [];

        internal bool AddProcessor<TUpdateData, TImplementation>(
            Func<Update, TUpdateData> dataSelector, UpdateType updateType)
            where TImplementation : class, IUpdateProcessor<TUpdateData> {
            services.AddScoped<IUpdateProcessor<TUpdateData>, TImplementation>();
            return Processors.TryAdd(updateType, ProcessorWrapper);

            Task ProcessorWrapper(IServiceProvider services, Update update, CancellationToken token) {
                var updateData = dataSelector(update);
                var processor = services.GetRequiredService<IUpdateProcessor<TUpdateData>>();
                return processor.Process(updateData, token);
            }
        }
    }
}
