namespace MephiWatcher;

[AttributeUsage(AttributeTargets.Class)]
internal class VuzParserNameAttribute(string name) : Attribute
{
    public string Name { get; set; } = name;
}
