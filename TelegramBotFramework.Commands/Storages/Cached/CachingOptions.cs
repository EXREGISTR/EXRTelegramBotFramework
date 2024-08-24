namespace TelegramBotFramework.Commands.Storages.Cached {
    public record CachingOptions(TimeSpan SlidingCachingTime, TimeSpan AbsoluteCachingTime);
}
