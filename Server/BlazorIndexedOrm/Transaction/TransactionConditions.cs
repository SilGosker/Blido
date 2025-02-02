using System.Linq.Expressions;
using Microsoft.JSInterop;

namespace BlazorIndexedOrm.Core.Transaction;

public class TransactionConditions<TEntity> where TEntity : class
{
    private List<Func<TEntity, bool>>? _conditions = null;
    public bool HasConditions => _conditions?.Count > 0;
    public void AddCondition(Expression<Func<TEntity, bool>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        _conditions ??= new();
        _conditions.Add(expression.Compile());
    }

    [JSInvokable]
    public bool FullFillsConditions(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (_conditions is null or { Count: 0 }) return true;

        return _conditions.All(condition => condition.Invoke(entity));
    }
}