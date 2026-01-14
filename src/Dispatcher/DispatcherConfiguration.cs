using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Dispatcher;

public class DispatcherConfiguration
{
    /// <summary>Which assemblies to scan for handlers & behaviors</summary>
    public List<Assembly> AssembliesToRegister { get; } = [];

    public List<ServiceDescriptor> BehaviorsToRegister { get; } = [];

    /// <summary>Lifetime to use for all handler & behavior registrations</summary>
    public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient;

    /// <summary>
    /// Optional filter for types to register. Default value is a function returning true.
    /// </summary>
    public Func<Type, bool> TypeEvaluator { get; set; } = _ => true;

    /// <summary>
    /// Register various handlers from assembly containing given type
    /// </summary>
    /// <typeparam name="T">Type from assembly to scan</typeparam>
    /// <returns>This</returns>
    public DispatcherConfiguration RegisterServicesFromAssemblyContaining<T>()
    {
        return RegisterServicesFromAssemblyContaining(typeof(T));
    }

    /// <summary>
    /// Register various handlers from assembly containing given type
    /// </summary>
    /// <param name="type">Type from assembly to scan</param>
    /// <returns>This</returns>
    public DispatcherConfiguration RegisterServicesFromAssemblyContaining(Type type)
    {
        return RegisterServicesFromAssembly(type.Assembly);
    }

    /// <summary>
    /// Register various handlers from assembly
    /// </summary>
    /// <param name="assembly">Assembly to scan</param>
    /// <returns>This</returns>
    public DispatcherConfiguration RegisterServicesFromAssembly(Assembly assembly)
    {
        AssembliesToRegister.Add(assembly);

        return this;
    }

    /// <summary>
    /// Register various handlers from assemblies
    /// </summary>
    /// <param name="assemblies">Assemblies to scan</param>
    /// <returns>This</returns>
    public DispatcherConfiguration RegisterServicesFromAssemblies(
        params Assembly[] assemblies)
    {
        AssembliesToRegister.AddRange(assemblies);

        return this;
    }

    // <summary>
    /// Register a closed behavior type
    /// </summary>
    /// <typeparam name="TServiceType">Closed behavior interface type</typeparam>
    /// <typeparam name="TImplementationType">Closed behavior implementation type</typeparam>
    /// <param name="serviceLifetime">Optional service lifetime, defaults to <see cref="ServiceLifetime.Transient"/>.</param>
    /// <returns>This</returns>
    public DispatcherConfiguration AddBehavior<TServiceType, TImplementationType>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        return AddBehavior(typeof(TServiceType), typeof(TImplementationType), serviceLifetime);
    }

    /// <summary>
    /// Register a closed behavior type
    /// </summary>
    /// <param name="serviceType">Closed behavior interface type</param>
    /// <param name="implementationType">Closed behavior implementation type</param>
    /// <param name="serviceLifetime">Optional service lifetime, defaults to <see cref="ServiceLifetime.Transient"/>.</param>
    /// <returns>This</returns>
    public DispatcherConfiguration AddBehavior(Type serviceType, Type implementationType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        BehaviorsToRegister.Add(new ServiceDescriptor(serviceType, implementationType, serviceLifetime));
        return this;
    }
}