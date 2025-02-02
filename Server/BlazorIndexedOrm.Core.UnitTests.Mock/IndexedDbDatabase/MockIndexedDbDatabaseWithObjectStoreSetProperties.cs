using Microsoft.JSInterop;

namespace BlazorIndexedOrm.Core.UnitTests.Mock.IndexedDbDatabase;

public class MockIndexedDbDatabaseWithObjectStoreSetProperties : Core.IndexedDbDatabase
{
    public MockIndexedDbDatabaseWithObjectStoreSetProperties(IJSRuntime jsRuntime) : base(jsRuntime)
    {
    }

    public ObjectStoreSet<string> Strings { get; set; } = null!;
    public ObjectStoreSet<Exception> Exceptions { get; set; } = null!;
}