using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace MephiWatcher;

public partial class Snils : AbiturDocument, IEquatable<Snils>
{
    [GeneratedRegex("^\\s*(\\d{3})[\\s-]?(\\d{3})[\\s-]?(\\d{3})[\\s-]?(\\d{2})\\s*$", RegexOptions.Compiled)]
    private static partial Regex MyRegex();

    private readonly ushort _n1, _n2, _n3;
    private readonly byte _n4;

    private Snils(Match match)
    {
        _n1 = ushort.Parse(match.Groups[1].Value);
        _n2 = ushort.Parse(match.Groups[2].Value);
        _n3 = ushort.Parse(match.Groups[3].Value);
        _n4 = byte.Parse(match.Groups[4].Value);
    }

    public Snils(ushort n1, ushort n2, ushort n3, byte n4)
    {
        (_n1, _n2, _n3, _n4) = (n1, n2, n3, n4);
    }

    public static Snils Parse(string repr)
    {
        bool success = TryParse(repr, out Snils? s);
        if (!success)
            throw new ArgumentException("This does not look like valid SNILS");
        return s!;
    }

    public static bool TryParse(string repr, [NotNullWhen(true)] out Snils? value)
    {
        value = null;
        var match = MyRegex().Match(repr);
        if (!match.Success)
            return false;
        value = new Snils(match);
        return true;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Snils other)
            return false;
        return Equals(other);
    }

    public override int GetHashCode()
    {
        return _n1 + _n2 + _n3 + _n4;
    }

    public bool Equals(Snils? other)
    {
        return other is not null && _n1 == other._n1 && _n2 == other._n2 && _n3 == other._n3 && _n4 == other._n4;
    }

    public override string ToString() => $"{_n1}-{_n2}-{_n3} {_n4}";
}
