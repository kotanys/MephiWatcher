namespace MephiWatcher;

public record Config
{
    public required int TotalPoints { get; init; }
    public required string Document { get; init; }
    public required VuzConfig[] VuzConfigs { get; init; }
}

public record VuzConfig(
    string UserFriendlyName,
    string Name,
    Uri Url,
    string[] ProgramNames);