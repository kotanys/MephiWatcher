namespace MephiWatcher;

public record Config
{
    public required int TotalPoints { get; init; }
    public required AbiturDocument Document { get; init; }
    public required VuzConfig[] VuzConfigs { get; init; }
}

public record VuzConfig(
    string UserFriendlyName,
    string Name,
    Uri Url,
    string[] ProgramNames);