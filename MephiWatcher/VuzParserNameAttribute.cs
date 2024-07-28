using MephiWatcher.Parsers;

namespace MephiWatcher;

/// <summary>
/// Specifies the name of the university whose website is parsed by this <see cref="IVuzParser"/> implementation.
/// </summary>
/// <param name="name">University name.</param>
[AttributeUsage(AttributeTargets.Class)]
internal class VuzParserNameAttribute(string name) : Attribute
{
    public string Name { get; set; } = name;
}
