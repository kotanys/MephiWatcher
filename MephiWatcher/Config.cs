using MephiWatcher.Parsers;

namespace MephiWatcher;

/// <summary>
/// Configuration for university watcher program.
/// </summary>
public record Config
{
    public required int TotalPoints { get; init; }
    public required AbiturDocument Document { get; init; }
    public required UniversityConfig[] UniversityConfigs { get; init; }
}

/// <summary>
/// Configuration for the specific university.
/// </summary>
/// <param name="UserFriendlyName">Name of the university that should be shown for the user.</param>
/// <param name="Name">Internal name that is used to associate this config with <see cref="IVuzParser"/>.</param>
/// <param name="Url">University's website.</param>
/// <param name="ProgramNames">Name of university programs that are of interest to user.</param>
public record UniversityConfig(
    string UserFriendlyName,
    string Name,
    Uri Url,
    string[] ProgramNames);