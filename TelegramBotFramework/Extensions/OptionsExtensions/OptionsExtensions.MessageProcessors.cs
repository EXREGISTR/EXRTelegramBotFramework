using System.Runtime.CompilerServices;
using Telegram.Bot.Types.Enums;
using TelegramBotFramework.Options;
using TelegramBotFramework.Processors.Contracts;

namespace TelegramBotFramework {
    public static class FrameworkOptionsExtensions {
        public static TelegramBotFrameworkOptions AddTextProcessor<T>(
            this TelegramBotFrameworkOptions source, ChatType[] availableChatTypes)
            where T : class, IMessageProcessor
            => source.AddMessageProcessorInternal<T>(MessageType.Text, availableChatTypes);

        public static TelegramBotFrameworkOptions AddPhotoProcessor<T>(
            this TelegramBotFrameworkOptions source, ChatType[] availableChatTypes)
            where T : class, IMessageProcessor
            => source.AddMessageProcessorInternal<T>(MessageType.Photo, availableChatTypes);

        public static TelegramBotFrameworkOptions AddAudioProcessor<T>(
            this TelegramBotFrameworkOptions source, ChatType[] availableChatTypes)
            where T : class, IMessageProcessor
            => source.AddMessageProcessorInternal<T>(MessageType.Audio, availableChatTypes);

        public static TelegramBotFrameworkOptions AddVideoProcessor<T>(
            this TelegramBotFrameworkOptions source, ChatType[] availableChatTypes)
            where T : class, IMessageProcessor
            => source.AddMessageProcessorInternal<T>(MessageType.Video, availableChatTypes);

        public static TelegramBotFrameworkOptions AddVoiceProcessor<T>(
            this TelegramBotFrameworkOptions source, ChatType[] availableChatTypes)
            where T : class, IMessageProcessor
            => source.AddMessageProcessorInternal<T>(MessageType.Voice, availableChatTypes);

        public static TelegramBotFrameworkOptions AddDocumentProcessor<T>(
            this TelegramBotFrameworkOptions source, ChatType[] availableChatTypes)
            where T : class, IMessageProcessor
            => source.AddMessageProcessorInternal<T>(MessageType.Document, availableChatTypes);

        public static TelegramBotFrameworkOptions AddStickerProcessor<T>(
            this TelegramBotFrameworkOptions source, ChatType[] availableChatTypes)
            where T : class, IMessageProcessor
            => source.AddMessageProcessorInternal<T>(MessageType.Sticker, availableChatTypes);
        
        public static TelegramBotFrameworkOptions AddLocationProcessor<T>(
            this TelegramBotFrameworkOptions source, ChatType[] availableChatTypes)
            where T : class, IMessageProcessor
            => source.AddMessageProcessorInternal<T>(MessageType.Location, availableChatTypes);
        
        public static TelegramBotFrameworkOptions AddContactProcessor<T>(
            this TelegramBotFrameworkOptions source, ChatType[] availableChatTypes)
            where T : class, IMessageProcessor
            => source.AddMessageProcessorInternal<T>(MessageType.Contact, availableChatTypes);
        
        public static TelegramBotFrameworkOptions AddVenueProcessor<T>(
            this TelegramBotFrameworkOptions source, ChatType[] availableChatTypes)
            where T : class, IMessageProcessor
            => source.AddMessageProcessorInternal<T>(MessageType.Venue, availableChatTypes);
        
        public static TelegramBotFrameworkOptions AddGameProcessor<T>(
            this TelegramBotFrameworkOptions source, ChatType[] availableChatTypes)
            where T : class, IMessageProcessor
            => source.AddMessageProcessorInternal<T>(MessageType.Game, availableChatTypes);

        public static TelegramBotFrameworkOptions AddVideoNoteProcessor<T>(
            this TelegramBotFrameworkOptions source, ChatType[] availableChatTypes)
            where T : class, IMessageProcessor
            => source.AddMessageProcessorInternal<T>(MessageType.VideoNote, availableChatTypes);

        public static TelegramBotFrameworkOptions AddInvoiceProcessor<T>(
            this TelegramBotFrameworkOptions source, ChatType[] availableChatTypes)
            where T : class, IMessageProcessor
            => source.AddMessageProcessorInternal<T>(MessageType.Invoice, availableChatTypes);

        public static TelegramBotFrameworkOptions AddFAFMessageProcessor<T>(
            this TelegramBotFrameworkOptions source, ChatType[] availableChatTypes)
            where T : class, IMessageProcessor
            => source.AddMessageProcessorInternal<T>(MessageType.Game, availableChatTypes);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TelegramBotFrameworkOptions AddMessageProcessorInternal<T>(
            this TelegramBotFrameworkOptions source, MessageType messageType, ChatType[] availableChatTypes)
            where T : class, IMessageProcessor {
            source.AddMessageProcessor<T>(messageType, availableChatTypes);
            return source;
        }
    }
}
