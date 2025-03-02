using Blido.Core.Attributes;
using Blido.Core.Transaction;

namespace Blido.Core;

[IndexedDbDatabaseName("CustomName")]
public class MockIndexedDbDatabaseWithAttribute : IndexedDbContext
{
    public MockIndexedDbDatabaseWithAttribute(IObjectStoreFactory transactionProviderFactory) : base(transactionProviderFactory)
    {
    }
}