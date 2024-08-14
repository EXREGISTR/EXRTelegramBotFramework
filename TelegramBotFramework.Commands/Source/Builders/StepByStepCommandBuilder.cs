using Microsoft.Extensions.DependencyInjection;
using TelegramBotFramework.Commands.Proxies.Contracts;
using TelegramBotFramework.Commands.Proxies.Steps;
using TelegramBotFramework.Commands.Storages.Contracts;

namespace TelegramBotFramework.Commands.Builders {
    public class StepByStepCommandBuilder<TData> : CommandBuilder where TData : class {
        private readonly string commandCode;
        private readonly IServiceCollection services;

        private CommandStepIdentity? lastStepIdentity;
        private bool configurationIsFinished;

        internal StepByStepCommandBuilder(
            string commandCode, 
            CommandEntry entry,
            IServiceCollection services) 
            : base(entry) {
            this.commandCode = commandCode;
            this.services = services;
        }

        public StepByStepCommandBuilder<TData> Step<TStep>(Func<IServiceProvider, object?, TStep>? factory = null)
            where TStep : class, ICommandStep<TData> {
            EnsureConfigurationIsNotFinished();

            var isFirstStep = lastStepIdentity == null;

            var identity = isFirstStep 
                ? CommandStepIdentity.First(commandCode) 
                : CommandStepIdentity.Next(lastStepIdentity!);

            RegisterStep(identity, factory);

            if (isFirstStep) {
                services.AddKeyedScoped<ICommandStepProxy>(identity,
                    (services, key) => {
                        var userStates = services.GetRequiredService<IUserCommandStatesStorage>();
                        var dataStorage = services.GetRequiredService<ICommandStepsDataStorage>();
                        return new FirstCommandStepProxy<TData>(identity, dataStorage, userStates, services);
                    });
            } else {
                services.AddKeyedScoped<ICommandStepProxy>(identity,
                    (services, key) => {
                        var userStates = services.GetRequiredService<IUserCommandStatesStorage>();
                        var dataStorage = services.GetRequiredService<ICommandStepsDataStorage>();
                        return new CommandStepProxy<TData>(identity, dataStorage, userStates, services);
                    });
            }

            lastStepIdentity = identity;
            return this;
        }

        public CommandBuilder EndStep<TStep>(Func<IServiceProvider, object?, TStep>? factory = null) 
            where TStep : class, ICommandStep<TData> {
            EnsureConfigurationIsNotFinished();

            var isFirstStep = lastStepIdentity == null;
            var identity = isFirstStep
                ? CommandStepIdentity.First(commandCode)
                : CommandStepIdentity.Next(lastStepIdentity!);

            RegisterStep(identity, factory);

            services.AddKeyedScoped<ICommandStepProxy>(identity,
                   (services, key) => {
                       var userStates = services.GetRequiredService<IUserCommandStatesStorage>();
                       var dataStorage = services.GetRequiredService<ICommandStepsDataStorage>();
                       return new LastCommandStepProxy<TData>(identity, dataStorage, userStates, services);
                   });

            configurationIsFinished = true;
            return this;
        }
        
        private void RegisterStep<TStep>(
            CommandStepIdentity identity, 
            Func<IServiceProvider, object?, TStep>? factory = null)
            where TStep : class, ICommandStep<TData> {

            if (factory == null) {
                services.AddKeyedScoped<ICommandStep<TData>, TStep>(identity);
            } else {
                services.AddKeyedScoped<ICommandStep<TData>, TStep>(identity, factory);
            }
        }

        private void EnsureConfigurationIsNotFinished() {
            if (configurationIsFinished) {
                throw new InvalidOperationException(
                    $"You already finished configuration of step by step command {commandCode}!");
            }
        }
    }
}
