namespace Blido.Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class FieldNameAttribute : Attribute
{
    public FieldNameAttribute(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name;
    }

    public string Name { get; }
}