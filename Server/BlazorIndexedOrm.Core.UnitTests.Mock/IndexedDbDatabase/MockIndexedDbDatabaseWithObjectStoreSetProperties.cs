using BlazorIndexedOrm.Core.Transaction;

namespace BlazorIndexedOrm.Core.UnitTests.Mock.IndexedDbDatabase;

public class MockIndexedDbDatabaseWithObjectStoreSetProperties : Core.IndexedDbDatabase
{
    public MockIndexedDbDatabaseWithObjectStoreSetProperties(IIndexedDbTransactionProviderFactory transactionProviderFactory)
        : base(transactionProviderFactory)
    {
    }

    public ObjectStoreSet<string> Strings { get; set; } = null!;
    public ObjectStoreSet<Exception> Exceptions { get; set; } = null!;
}