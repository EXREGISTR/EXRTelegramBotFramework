using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Payments;
using TelegramBotFramework.UpdateProcessors;

namespace TelegramBotFramework.Extensions {
    public static class UpdateProcessorsInjection {
        public static IRequestConveyer<Message> AddMessageProcessing(
            this IServiceCollection services) =>
            services.AddUpdateProcessing(
                update => update.Message!,
                UpdateType.Message);

        public static IRequestConveyer<Message> AddEditedMessageProcessing(
            this IServiceCollection services) =>
            services.AddUpdateProcessing(
                update => update.EditedMessage!,
                UpdateType.EditedMessage);

        public static IRequestConveyer<Message> AddChannelPostProcessing(
            this IServiceCollection services) =>
            services.AddUpdateProcessing(
                update => update.ChannelPost!,
                UpdateType.ChannelPost);

        public static IRequestConveyer<Message> AddEditedChannelPostProcessing(
            this IServiceCollection services) =>
            services.AddUpdateProcessing(
                update => update.EditedChannelPost!,
                UpdateType.EditedChannelPost);

        public static IRequestConveyer<InlineQuery> AddInlineQueryProcessing(
            this IServiceCollection services) =>
            services.AddUpdateProcessing(
                update => update.InlineQuery!,
                UpdateType.InlineQuery);

        public static IRequestConveyer<CallbackQuery> AddCallbackQueryProcessing(
            this IServiceCollection services) =>
            services.AddUpdateProcessing(
                update => update.CallbackQuery!,
                UpdateType.CallbackQuery);

        public static IRequestConveyer<ChosenInlineResult> AddChosenInlineResultProcessing(
            this IServiceCollection services) =>
            services.AddUpdateProcessing(
                update => update.ChosenInlineResult!,
                UpdateType.ChosenInlineResult);


        public static IRequestConveyer<Poll> AddPollProcessing(
            this IServiceCollection services) =>
            services.AddUpdateProcessing(
                update => update.Poll!,
                UpdateType.Poll);

        public static IRequestConveyer<PollAnswer> AddPollAnswerProcessing(
            this IServiceCollection services) =>
            services.AddUpdateProcessing(
                update => update.PollAnswer!,
                UpdateType.PollAnswer);

        public static IRequestConveyer<ChatMemberUpdated> AddChatMemberProcessing(
            this IServiceCollection services) =>
            services.AddUpdateProcessing(
                update => update.ChatMember!,
                UpdateType.ChatMember);

        public static IRequestConveyer<ChatMemberUpdated> AddMyChatMemberProcessing(
            this IServiceCollection services) =>
            services.AddUpdateProcessing(
                update => update.MyChatMember!,
                UpdateType.MyChatMember);

        public static IRequestConveyer<ChatJoinRequest> AddChatJoinRequestProcessing(
            this IServiceCollection services) =>
            services.AddUpdateProcessing(
                update => update.ChatJoinRequest!,
                UpdateType.ChatJoinRequest);


        public static IRequestConveyer<ShippingQuery> AddShippingQueryProcessing(
            this IServiceCollection services) =>
            services.AddUpdateProcessing(
                update => update.ShippingQuery!,
                UpdateType.ShippingQuery);

        public static IRequestConveyer<PreCheckoutQuery> AddPreCheckoutQueryProcessing(
            this IServiceCollection services) =>
            services.AddUpdateProcessing(
                update => update.PreCheckoutQuery!,
                UpdateType.PreCheckoutQuery);

        private static RequestConveyer<TData> AddUpdateProcessing<TData>(this IServiceCollection services,
            Func<Update, TData> dataSelector,
            UpdateType updateType) {
            var conveyer = new RequestConveyer<TData>(dataSelector);
            services.AddKeyedSingleton<IUpdateProcessor>(updateType, conveyer);
            return conveyer;
        }
    }
}
