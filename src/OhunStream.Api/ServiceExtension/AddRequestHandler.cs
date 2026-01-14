using CurriculumService.Application.Behaviours;
using Dispatcher;
using static OhunStream.Application.Commands.StartSessionCommand;
using FluentValidation.Validators;
using FluentValidation;


namespace OhunStream.Api.ServiceExtension
{
    public static class AddRequestHandler
    {
        public static IServiceCollection AddRequestHandlers(this IServiceCollection services)
        {
            var applicationAssembly = typeof(StartSessionRequest).Assembly;
            services.AddOhunStreamDispatcher(c => c.RegisterServicesFromAssemblies(applicationAssembly)
                .AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>)));
            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddValidatorsFromAssemblyContaining<CreateSessionCommandValidator>();
        }
    }   
}
