namespace TelegramBotFramework.Commands.Storages.Options {
    public record CachingOptions(TimeSpan CachingTime, CachingType Type);

    public enum CachingType { SlidingExpiration, AbsoluteExpiration }
}
