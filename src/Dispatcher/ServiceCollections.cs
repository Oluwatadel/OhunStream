using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dispatcher
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOhunStreamDispatcher(this IServiceCollection services,
            Action<DispatcherConfiguration> configuration)
        {
            var serviceConfig = new DispatcherConfiguration();
            configuration.Invoke(serviceConfig);
            return services.AddOhunStreamDispatcher(serviceConfig);
        }

        public static IServiceCollection AddOhunStreamDispatcher(this IServiceCollection services, DispatcherConfiguration configuration)
        {
            if (!configuration.AssembliesToRegister.Any())
            {
                throw new ArgumentException("No assemblies found to scan. Supply at least one assembly to scan for handlers.");
            }
            AddRequiredServices(services, configuration);
            return services;
        }

        private static IServiceCollection AddRequiredServices(this IServiceCollection services, DispatcherConfiguration serviceConfiguration)
        {
            services.TryAdd(new ServiceDescriptor(typeof(ISender), typeof(Sender), serviceConfiguration.Lifetime));
            var handlerInterfaces = new HashSet<Type>
        {
            typeof(IRequestHandler<,>),
            typeof(IRequestHandler<>)
        };
            var types = serviceConfiguration.AssembliesToRegister
                .SelectMany(a => a.GetTypes())
                .Where(serviceConfiguration.TypeEvaluator)
                .Where(t => t is { IsClass: true, IsAbstract: false });
            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces()
                    .Where(i =>
                        i.IsGenericType && handlerInterfaces.Contains(i.GetGenericTypeDefinition()));
                foreach (var iface in interfaces)
                {
                    services.Add(new ServiceDescriptor(iface, type, serviceConfiguration.Lifetime));
                }
            }
            foreach (var serviceDescriptor in serviceConfiguration.BehaviorsToRegister)
            {
                services.TryAddEnumerable(serviceDescriptor);
            }
            return services;
        }
    }
}
