namespace MephiWatcher;

public record VuzProgram(
    string Name,
    Uri Url
    );

public record ProgramRating(
    VuzProgram Program,
    IEnumerable<Entry> Entries
    );

public record Entry(
    VuzProgram Program,
    int SerialNumber,
    string Document,
    int Points,
    Status Status
    );

public record EntryDto(
    int SerialNumber,
    int TotalNumber,
    VuzProgram Program,
    Status Status);

public enum Status
{
    Pass,
    Possible,
    Fail
}

public record Config
{
    public required Uri RatingUrl { get; init; }
    public required int TotalPoints { get; init; }
    public required string Document { get; init; }
    public required List<string> ProgramNames { get; init; }
}