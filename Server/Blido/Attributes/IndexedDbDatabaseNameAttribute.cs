namespace Blido.Core.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class IndexedDbDatabaseNameAttribute : Attribute
{
    public string Name { get; }
    public IndexedDbDatabaseNameAttribute(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name;
    }
}