using Microsoft.JSInterop;

namespace BlazorIndexedOrm.Core.UnitTests.Mock.IndexedDbDatabase;

public class MockIndexedDbDatabase : Core.IndexedDbDatabase
{
    public MockIndexedDbDatabase(IJSRuntime jsRuntime) : base(jsRuntime)
    {
    }
}