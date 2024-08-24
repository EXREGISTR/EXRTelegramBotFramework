namespace TelegramBotFramework.Commands {
    internal record CommandStateIdentity {
        public string Key { get; init; }

        private CommandStateIdentity(string key) => Key = key;

        public static CommandStateIdentity Create(Type stateType) => new(stateType.Name);
    }



    // var builder = services.AddCommands();
    // builder
    //     .AddStatelessCommand<CreateWarriorCommand>() Это будет конечный обработчик, когда все стейты выполнил
    //     .Parameterize<WarriorCreationContext>() создается билдер который заставляет регать стейты IState<TData>
    //     .AddState<>
    // 
}
