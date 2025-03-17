using System.Text.Json;
using Blido.Core.Transaction.JsExpression;
using Microsoft.JSInterop;

namespace Blido.Core.Transaction.Materialization;

public class IdentifierMaterializer
{
    public static async Task<TResult> ExecuteAsync<TEntity, TResult>(IJSRuntime jsRuntime,
        ObjectStore<TEntity> objectStore,
        IExpressionBuilder expressionBuilder,
        object identifiers,
        string methodName,
        CancellationToken cancellationToken)
        where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(identifiers);
        ArgumentException.ThrowIfNullOrWhiteSpace(methodName);

        string arguments = JsonSerializer.Serialize(new MaterializationArguments()
        {
            Database = objectStore.Database.Name,
            ObjectStore = objectStore.Name,
            Version = await objectStore.Database.GetVersionAsync(cancellationToken),
            Identifiers = identifiers
        }, MaterializationArgumentsSerializationContext.Default.MaterializationArguments);

        return await jsRuntime.InvokeAsync<TResult>(methodName, cancellationToken, arguments);
    }
}