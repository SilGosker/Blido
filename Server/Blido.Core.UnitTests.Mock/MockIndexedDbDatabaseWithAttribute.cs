using Blido.Core.Attributes;
using Blido.Core.Transaction;
using Microsoft.JSInterop;

namespace Blido.Core;

[IndexedDbDatabaseName("CustomName")]
public class MockIndexedDbDatabaseWithAttribute : IndexedDbDatabase
{
    public MockIndexedDbDatabaseWithAttribute(IIndexedDbTransactionProviderFactory transactionProviderFactory) : base(transactionProviderFactory)
    {
    }
}