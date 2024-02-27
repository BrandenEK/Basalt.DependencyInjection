
namespace Basalt.DependencyInjection;

public interface ILogger
{
    void Info(object message);

    void Warn(object message);

    void Error(object message);
}

public class ConsoleLogger : ILogger
{
    public void Info(object message)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(message);
    }

    public void Warn(object message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message);
    }

    public void Error(object message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
    }
}

public class FakeLogger : ILogger
{
    public void Info(object message)
    {
        Console.WriteLine("Fake logging");
    }

    public void Warn(object message)
    {
        Console.WriteLine("Fake logging");
    }

    public void Error(object message)
    {
        Console.WriteLine("Fake logging");
    }
}
