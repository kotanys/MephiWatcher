namespace MephiWatcher.Parsers
{
    public interface IVuzParser
    {
        Task<VuzProgram[]> ParseProgramsAsync(Uri url, CancellationToken ct);
        Task<ProgramRating> RarseProgramRatingAsync(VuzProgram program, CancellationToken ct);
    }
}
