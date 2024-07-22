namespace MephiWatcher;

public record VuzProgram(
    string Name,
    Uri Url
    );

public record ProgramRating(
    VuzProgram Program,
    Entry[] Entries
    );

public record Entry(
    VuzProgram Program,
    int SerialNumber,
    AbiturDocument Document,
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
