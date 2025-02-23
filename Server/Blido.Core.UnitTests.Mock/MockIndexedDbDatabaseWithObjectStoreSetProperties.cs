using Blido.Core.Transaction;

namespace Blido.Core;

public class MockIndexedDbDatabaseWithObjectStoreSetProperties : IndexedDbDatabase
{
    public MockIndexedDbDatabaseWithObjectStoreSetProperties(IIndexedDbTransactionProviderFactory transactionProviderFactory)
        : base(transactionProviderFactory)
    {
    }

    public ObjectStore<string> Strings { get; set; } = null!;
    public ObjectStore<Exception> Exceptions { get; set; } = null!;
}