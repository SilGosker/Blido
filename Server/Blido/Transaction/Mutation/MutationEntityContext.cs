using System.Reflection;
using Blido.Core.Helpers;

namespace Blido.Core.Transaction.Mutation;

public class MutationEntityContext
{
    public static MutationEntityContext Insert(object entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return new MutationEntityContext(entity, MutationState.Added);
    }

    public static MutationEntityContext Update(object entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return new MutationEntityContext(entity, MutationState.Modified);
    }

    public static MutationEntityContext Delete(object entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return new MutationEntityContext(entity, MutationState.Deleted);
    }

    private MutationEntityContext(object entity, MutationState state)
    {
        State = state;
        BeforeChange = entity;
        PrimaryKeys = KeyedPropertyHelper.GetKeys(entity.GetType()).ToArray();
    }

    public object BeforeChange { get; private set; }
    public object? AfterChange { get; internal set; }
    public MutationState State { get; }
    internal string StateMethodName => State switch
    {
        MutationState.Added => "InsertAsync",
        MutationState.Modified => "UpdateAsync",
        MutationState.Deleted => "DeleteAsync",
        _ => throw new ArgumentOutOfRangeException(nameof(State))
    };
    public PropertyInfo[] PrimaryKeys { get; private set; }
}