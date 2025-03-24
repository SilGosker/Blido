using Blido.Core.Transaction;
using Blido.Core.Transaction.Mutation.KeyGeneration.KeyGenerators;

namespace Blido.Core;

public class MockIndexedDbDatabaseWithPrimaryKeySetProperties : IndexedDbContext
{
    public MockIndexedDbDatabaseWithPrimaryKeySetProperties(IObjectStoreFactory transactionProviderFactory) : base(transactionProviderFactory)
    {
    }

    public ObjectStore<EntityWithGuidKey> GuidEntities { get; set; } = null!;
    public ObjectStore<EntityWithNumberKey> NumberEntities { get; set; } = null!;

}