using Blido.Core.Attributes;

namespace Blido.Core;

public class MockObjectStoreWithFieldAttribute
{
    [FieldName("__name")] public string Name { get; set; } = string.Empty;
}