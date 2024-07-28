namespace MephiWatcher.Parsers
{
    public interface IVuzParser
    {
        Task<UniversityProgram[]> ParseProgramsAsync(Uri url, CancellationToken ct);
        Task<ProgramRating> RarseProgramRatingAsync(UniversityProgram program, CancellationToken ct);
    }
}
