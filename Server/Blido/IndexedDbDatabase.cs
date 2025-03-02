using Blido.Core.Transaction;
using Microsoft.JSInterop;

namespace Blido.Core;

public class IndexedDbDatabase
{
    private readonly IJSRuntime _jsRuntime;

    public IndexedDbDatabase(IndexedDbContext dbContext, IJSRuntime jsRuntime)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        ArgumentNullException.ThrowIfNull(jsRuntime);
        _jsRuntime = jsRuntime;
        Name = NameResolver.ResolveIndexedDbName(dbContext.GetType());
    }

    public async ValueTask<ulong> GetVersionAsync(CancellationToken cancellationToken = default)
    {
        return await _jsRuntime.InvokeAsync<ulong>(JsMethodNames.GetVersion, cancellationToken);
    }

    public string Name { get; }
}