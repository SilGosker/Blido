using Blido.Core.Transaction;

namespace Blido.Core;

public class MockIndexedDbDatabase : IndexedDbContext
{
    public MockIndexedDbDatabase(IObjectStoreFactory transactionProviderFactory) : base(transactionProviderFactory)
    {
    }
}