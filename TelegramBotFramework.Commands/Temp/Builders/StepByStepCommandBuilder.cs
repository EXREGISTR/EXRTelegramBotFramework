using Microsoft.Extensions.DependencyInjection;
using TelegramBotFramework.Commands.Proxies.Contracts;
using TelegramBotFramework.Commands.Proxies.Steps;

namespace TelegramBotFramework.Commands.Temp.Builders
{
    public class StepByStepCommandBuilder<TCommand, TData> : CommandBuilder where TData : class
    {
        private CommandStateIdentity? lastStepIdentity;
        private bool configurationIsFinished;

        internal StepByStepCommandBuilder(
            CommandEntry entry)
            : base(entry)
        {

        }

        public StepByStepCommandBuilder<TCommand, TData> Step<TStep>(Func<IServiceProvider, object?, TStep>? factory = null)
            where TStep : class, ICommandStep<TData>
        {
            EnsureConfigurationIsNotFinished();

            var isFirstStep = lastStepIdentity == null;

            var identity = isFirstStep
                ? CommandStepIdentity.First(commandCode)
                : CommandStepIdentity.Next(lastStepIdentity!);

            RegisterStep(identity, factory);

            if (isFirstStep)
            {
                services.AddKeyedScoped<ICommandStateProxy>(identity,
                    (services, key) =>
                    {
                        var userStates = services.GetRequiredService<IUserActiveCommandStepsStorage>();
                        var dataStorage = services.GetRequiredService<ICommandStepsDataStorage>();
                        return new FirstCommandStepProxy<TData>(identity, dataStorage, userStates, services);
                    });
            }
            else
            {
                services.AddKeyedScoped<ICommandStateProxy>(identity,
                    (services, key) =>
                    {
                        var dataStorage = services.Get
                        return new CommandStepProxy<TData>(identity, dataStorage, userStates, services);
                    });
            }

            lastStepIdentity = identity;
            return this;
        }

        public CommandBuilder EndStep<TStep>(Func<IServiceProvider, object?, TStep>? factory = null)
            where TStep : class, ICommandStep<TData>
        {
            EnsureConfigurationIsNotFinished();

            var isFirstStep = lastStepIdentity == null;
            var identity = isFirstStep
                ? CommandStepIdentity.First(commandCode)
                : CommandStepIdentity.Next(lastStepIdentity!);

            RegisterStep(identity, factory);

            services.AddKeyedScoped<ICommandStateProxy>(identity,
                   (services, key) =>
                   {
                       var userStates = services.GetRequiredService<IUserActiveCommandStepsStorage>();
                       var dataStorage = services.GetRequiredService<ICommandStepsDataStorage>();
                       return new LastCommandStepProxy<TData>(identity, dataStorage, userStates, services);
                   });

            configurationIsFinished = true;
            return this;
        }

        private void RegisterStep<TStep>(
            CommandStepIdentity identity,
            Func<IServiceProvider, object?, TStep>? factory = null)
            where TStep : class, ICommandStep<TData>
        {

            if (factory == null)
            {
                services.AddKeyedScoped<ICommandStep<TData>, TStep>(identity);
            }
            else
            {
                services.AddKeyedScoped<ICommandStep<TData>, TStep>(identity, factory);
            }
        }

        private void EnsureConfigurationIsNotFinished()
        {
            if (configurationIsFinished)
            {
                throw new InvalidOperationException(
                    $"You already finished configuration of step by step command {commandCode}!");
            }
        }
    }
}
