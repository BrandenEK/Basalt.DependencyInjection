
namespace Basalt.DependencyInjection;

public class TestService
{
    private readonly ILogger _logger;

    public TestService(ILogger logger, IDataImporter importer)
    {
        _logger = logger;
        _logger.Error("Created test service");

        string[] data = importer.Import();
        foreach (var item in data)
        {
            _logger.Warn(item);
        }
    }
}
