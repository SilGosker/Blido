using Blido.Core.Transaction;

namespace Blido.Core;

public class MockIndexedDbDatabaseWithObjectStoreSetProperties : IndexedDbContext
{
    public MockIndexedDbDatabaseWithObjectStoreSetProperties(IObjectStoreFactory transactionProviderFactory)
        : base(transactionProviderFactory)
    {
    }

    public ObjectStore<string> Strings { get; set; } = null!;
    public ObjectStore<object> Objects { get; set; } = null!;
}