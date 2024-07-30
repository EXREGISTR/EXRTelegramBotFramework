namespace TelegramBotFramework.Tests {
    public static class ConfigureOptionsExtensions {
        public static IServiceCollection ConfigureForTypeName<TOptions>(
            this IServiceCollection services, IConfiguration configuration) 
            => services.ConfigureForTypeName<TOptions>(configuration.GetSection(typeof(TOptions).Name));
    }
}