using System.Linq.Expressions;

namespace BlazorIndexedOrm.Core.Transaction;

public class TransactionConditions<TEntity> where TEntity : class
{
    private List<Func<TEntity, bool>>? _conditions = null;
    public void AddCondition(Expression<Func<TEntity, bool>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        _conditions ??= new();
        _conditions.Add(expression.Compile());
    }

    public bool FullFillsConditions(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (_conditions is null or { Count: 0 }) return true;

        return _conditions.All(condition => condition.Invoke(entity));
    }
}