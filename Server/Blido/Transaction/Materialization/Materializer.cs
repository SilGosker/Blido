using System.Text.Json;
using Blido.Core.Transaction.Configuration;
using Blido.Core.Transaction.JsExpression;
using Microsoft.JSInterop;

namespace Blido.Core.Transaction.Materialization;

public class Materializer
{
    public static async Task<TResult> ExecuteAsync<TEntity, TResult>(IJSRuntime jsRuntime,
        ObjectStore<TEntity> objectStore,
        IExpressionBuilder expressionBuilder,
        TransactionConditions<TEntity> conditions,
        string methodName,
        CancellationToken cancellationToken)
    where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(conditions);
        ArgumentException.ThrowIfNullOrWhiteSpace(methodName);

        string[]? expressions = null;

        if (conditions.HasConditions)
        {
            expressions = new string[conditions.Count];
            for (int i = 0; i < conditions.Count; i++)
            {
                expressions[i] = expressionBuilder.ProcessExpression(conditions[i]!);
            }
        }

        string arguments = JsonSerializer.Serialize(new MaterializationArguments()
        {
            Database = objectStore.Database.Name,
            ObjectStore = objectStore.Name,
            Version = await objectStore.Database.GetVersionAsync(cancellationToken),
            ParsedExpressions = expressions
        }, MaterializationArgumentsSerializationContext.Default.MaterializationArguments);

        return await jsRuntime.InvokeAsync<TResult>(methodName, cancellationToken, arguments);
    }

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