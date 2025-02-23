using Blido.Core.Transaction;

namespace Blido.Core;

public class MockIndexedDbDatabase : IndexedDbDatabase
{
    public MockIndexedDbDatabase(IIndexedDbTransactionProviderFactory transactionProviderFactory) : base(transactionProviderFactory)
    {
    }
}