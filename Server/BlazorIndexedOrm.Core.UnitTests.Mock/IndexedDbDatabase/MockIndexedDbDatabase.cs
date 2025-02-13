using BlazorIndexedOrm.Core.Transaction;
using Microsoft.JSInterop;

namespace BlazorIndexedOrm.Core.UnitTests.Mock.IndexedDbDatabase;

public class MockIndexedDbDatabase : Core.IndexedDbDatabase
{
    public MockIndexedDbDatabase(IIndexedDbTransactionProviderFactory transactionProviderFactory) : base(transactionProviderFactory)
    {
    }
}