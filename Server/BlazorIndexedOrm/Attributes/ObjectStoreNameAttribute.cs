namespace BlazorIndexedOrm.Core.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ObjectStoreNameAttribute : Attribute
{
    public ObjectStoreNameAttribute(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name;
    }
    public string Name { get; }
}