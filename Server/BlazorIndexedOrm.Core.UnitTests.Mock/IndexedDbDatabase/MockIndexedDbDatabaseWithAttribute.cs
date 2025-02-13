using BlazorIndexedOrm.Core.Attributes;
using BlazorIndexedOrm.Core.Transaction;
using Microsoft.JSInterop;

namespace BlazorIndexedOrm.Core.UnitTests.Mock.IndexedDbDatabase;

[IndexedDbDatabaseName("CustomName")]
public class MockIndexedDbDatabaseWithAttribute : Core.IndexedDbDatabase
{
    public MockIndexedDbDatabaseWithAttribute(IIndexedDbTransactionProviderFactory transactionProviderFactory) : base(transactionProviderFactory)
    {
    }
}