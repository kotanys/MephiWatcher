using System.Text.Json;

namespace MephiWatcher;

public class ConfigFactory(string fileName) : IConfigFactory
{
    private readonly string _fileName = fileName;

    public Config Create()
    {
        var json = File.ReadAllText(_fileName);
        return JsonSerializer.Deserialize<Config>(json) ?? throw new ArgumentException("Couldn't deserialize config");
    }
}

public interface IConfigFactory
{
    Config Create();
}
