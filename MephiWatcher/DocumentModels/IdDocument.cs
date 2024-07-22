namespace MephiWatcher;

public class IdDocument(int id) : AbiturDocument
{
    private int Id { get; } = id;
    public override bool Equals(object? obj) => obj is IdDocument other && Id == other.Id;
    public override int GetHashCode() => Id;
    public override string ToString() => Id.ToString();
}
