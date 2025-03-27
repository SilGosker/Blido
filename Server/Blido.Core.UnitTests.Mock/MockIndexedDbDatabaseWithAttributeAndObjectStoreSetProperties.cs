using Blido.Core.Attributes;
using Blido.Core.Transaction;

namespace Blido.Core;

[IndexedDbDatabaseName("CustomName")]
public class MockIndexedDbDatabaseWithAttributeAndObjectStoreSetProperties : IndexedDbContext
{
    public MockIndexedDbDatabaseWithAttributeAndObjectStoreSetProperties(IObjectStoreFactory transactionProviderFactory) : base(transactionProviderFactory)
    {
    }

    public ObjectStore<MockObjectStoreWithAttribute> Objects { get; set; } = null!;
}