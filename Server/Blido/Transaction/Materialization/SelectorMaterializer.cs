using System.Linq.Expressions;
using System.Text.Json;
using Blido.Core.Transaction.Configuration;
using Blido.Core.Transaction.JsExpression;
using Microsoft.JSInterop;

namespace Blido.Core.Transaction.Materialization;

public class SelectorMaterializer
{
    public static async ValueTask<TResult> ExecuteAsync<TEntity, TResult>(IJSRuntime jsRuntime,
        ObjectStore<TEntity> objectStore,
        IExpressionBuilder expressionBuilder,
        TransactionConditions<TEntity> conditions,
        Expression<Func<TEntity, TResult>> selector,
        string methodName,
        CancellationToken cancellationToken)
        where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(conditions);
        ArgumentNullException.ThrowIfNull(selector);
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
        
        string expression = expressionBuilder.ProcessExpression(selector);

        string arguments = JsonSerializer.Serialize(new MaterializationArguments()
        {
            Database = objectStore.Database.Name,
            ObjectStore = objectStore.Name,
            Version = await objectStore.Database.GetVersionAsync(cancellationToken),
            ParsedExpressions = expressions,
            Selector = expression
        }, MaterializationArgumentsSerializationContext.Default.MaterializationArguments);

        return await jsRuntime.InvokeAsync<TResult>(methodName, cancellationToken, arguments);
    }
}