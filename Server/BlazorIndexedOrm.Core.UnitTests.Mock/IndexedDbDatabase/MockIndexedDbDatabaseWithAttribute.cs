using BlazorIndexedOrm.Core.Attributes;
using Microsoft.JSInterop;

namespace BlazorIndexedOrm.Core.UnitTests.Mock.IndexedDbDatabase;

[IndexedDbDatabaseName("CustomName")]
public class MockIndexedDbDatabaseWithAttribute : Core.IndexedDbDatabase
{
    public MockIndexedDbDatabaseWithAttribute(IJSRuntime jsRuntime) : base(jsRuntime)
    {
    }
}