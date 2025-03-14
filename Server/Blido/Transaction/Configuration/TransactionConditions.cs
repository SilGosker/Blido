using System.Linq.Expressions;
using Microsoft.JSInterop;

namespace Blido.Core.Transaction.Configuration;

public class TransactionConditions<TEntity> where TEntity : class
{
    private List<Expression<Func<TEntity, bool>>>? _conditions;
    public bool HasConditions => _conditions?.Count > 0;
    public int Count => _conditions?.Count ?? 0;

    public void AddCondition(Expression<Func<TEntity, bool>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        _conditions ??= new();
        _conditions.Add(expression);
    }

    public LambdaExpression? this[int index] => _conditions?[index];

    [JSInvokable]
    public bool FullFillsConditions(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (_conditions is null or { Count: 0 }) return true;

        return _conditions.All(condition => condition.Compile().Invoke(entity));
    }

    public void Clear()
    {
        _conditions?.Clear();
    }
}