
namespace Basalt.DependencyInjection;

public interface IDataImporter
{
    string[] Import();
}

public class FileImporter : IDataImporter
{
    private readonly string _path;

    public FileImporter(string path)
    {
        _path = path;
    }

    public string[] Import()
    {
        return File.Exists(_path) ? File.ReadAllLines(_path) : Array.Empty<string>();
    }
}

public class RandomImporter : IDataImporter
{
    private readonly ILogger _logger;

    public RandomImporter(ILogger logger)
    {
        _logger = logger;
    }

    public string[] Import()
    {
        _logger.Error("Why are you using random data");
        return new string[]
        {
            "random data 1",
            "random data 2"
        };
    }
}
