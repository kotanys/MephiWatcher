namespace MephiWatcher;

public static class AbiturDocumentFactory
{
    public static AbiturDocument Create(string repr)
    {
        if (Snils.TryParse(repr, out Snils? s))
            return s;
        if (int.TryParse(repr, out int id))
            return new IdDocument(id);
        throw new ArgumentException("Unable to create " + nameof(AbiturDocument));
    }
}
