namespace Basalt.DependencyInjection;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        string filePath = Path.Combine(Environment.CurrentDirectory, "input.txt");

        var injector = new DependencyInjector();

        injector.AddService<TestService>();

        injector.AddDependency<ILogger, ConsoleLogger>();
        injector.AddDependency<IDataImporter, FileImporter>(() => new FileImporter(filePath));

        injector.Run();

        Console.ReadKey();
    }
}
