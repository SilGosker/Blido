using BlazorIndexedOrm.Core.Attributes;

namespace BlazorIndexedOrm.Core.UnitTests.Mock.ObjectStore.ObjectStore;

public class MockObjectStoreWithFieldAttribute
{
    [FieldName("__name")] public string Name { get; set; } = string.Empty;
}