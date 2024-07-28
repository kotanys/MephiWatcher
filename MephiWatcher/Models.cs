namespace MephiWatcher;


public record UniversityProgram(
    string Name,
    Uri Url
    );

public record ProgramRating(
    UniversityProgram Program,
    Entry[] Entries
    );

public record Entry(
    UniversityProgram Program,
    int SerialNumber,
    AbiturDocument Document,
    int Points,
    Status Status
    );

public record EntryDto(
    int SerialNumber,
    int TotalNumber,
    UniversityProgram Program,
    Status Status);

public enum Status
{
    Pass,
    Possible,
    Fail
}
