using System.Reflection;

namespace Basalt.DependencyInjection;

public class DependencyInjector
{
    private readonly List<Service> _services = new();
    private readonly List<Dependency> _dependencies = new();

    private readonly Dictionary<Type, object> _instances = new();

    public void AddService<T>()
    {
        _services.Add(new Service(typeof(T)));
    }

    public void AddDependency<I, T>() where I : class where T : class
    {
        AddDependency(typeof(I), typeof(T), () => CreateInstanceWithDependencies(typeof(T)));
    }

    public void AddDependency<I, T>(Func<T> producer) where I : class where T : class
    {
        AddDependency(typeof(I), typeof(T), producer);
    }

    private void AddDependency(Type interfaceType, Type concreteType, Func<object> producer)
    {
        if (!interfaceType.IsInterface)
            throw new ArgumentException($"Type {interfaceType.Name} is not an interface");

        if (!interfaceType.IsAssignableFrom(concreteType))
            throw new ArgumentException($"Type {concreteType.Name} does not derive from {interfaceType.Name}");

        _dependencies.Add(new Dependency(interfaceType, producer));
    }

    public I GetService<I>() where I : class
    {
        Type interfaceType = typeof(I);

        if (!_dependencies.Any(d => d.Interface == interfaceType))
            throw new ArgumentException($"Type {interfaceType.Name} is not injectable");

        return (I)GetDependency(interfaceType);
    }

    public void Run()
    {
        foreach (var service in _services)
        {
            CreateInstanceWithDependencies(service.Class);
        }
    }

    private object CreateInstanceWithDependencies(Type concreteType)
    {
        Console.WriteLine($"Attempting to create {concreteType.Name}");
        var constructor = GetValidConstructor(concreteType);
        object[] args = constructor.GetParameters().Select(p => GetDependency(p.ParameterType)).ToArray();

        Console.WriteLine($"Created instance of {concreteType.Name}");
        return constructor.Invoke(args);
    }

    private object GetDependency(Type interfaceType)
    {
        Console.WriteLine($"Finding dependency {interfaceType.Name}");
        if (_instances.TryGetValue(interfaceType, out object? instance))
            return instance;

        Console.WriteLine($"No instance of {interfaceType.Name} exists yet");
        return _instances[interfaceType] = _dependencies.First(d => d.Interface == interfaceType).Producer();
    }

    private ConstructorInfo GetValidConstructor(Type concreteType)
    {
        var constructors = concreteType.GetConstructors();

        if (constructors.Length == 0)
            throw new Exception($"Type {concreteType.Name} has no constructor");

        if (constructors.Length > 1)
            throw new Exception($"Type {concreteType.Name} has multiple constructors");

        if (constructors[0].GetParameters().Any(p => !_dependencies.Any(d => d.Interface == p.ParameterType)))
            throw new Exception($"Type {concreteType.Name} has no injectable constructor");

        Console.WriteLine($"Found valid constructor for {concreteType.Name}");
        return constructors[0];
    }

    record Service(Type Class);
    record Dependency(Type Interface, Func<object> Producer);
}
