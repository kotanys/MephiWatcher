using System.Text.Json;

namespace MephiWatcher;

/// <summary>
/// This <see cref="IConfigFactory"/> implementation reads predetermined Json file and parses it as <see cref="Config"/>.
/// </summary>
/// <param name="fileName">Json file name.</param>
public class ConfigFactory(string fileName) : IConfigFactory
{
    private readonly string _fileName = fileName;

    public Config Create()
    {
        var json = File.ReadAllText(_fileName);
        return JsonSerializer.Deserialize<Config>(json) ?? throw new ArgumentException("Couldn't deserialize config");
    }
}

/// <summary>
/// Interface for a type that gets global <see cref="Config"/> from some source.
/// </summary>
public interface IConfigFactory
{
    Config Create();
}
