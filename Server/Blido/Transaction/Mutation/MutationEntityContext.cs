using System.Reflection;
using Blido.Core.Helpers;

namespace Blido.Core.Transaction.Mutation;

public class MutationEntityContext
{
    public MutationEntityContext(object entity, MutationState state)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (!Enum.IsDefined(state))
        {
            throw new ArgumentOutOfRangeException(nameof(state));
        }
        State = state;
        BeforeChange = entity;
        PrimaryKeys = KeyedPropertyHelper.GetKeys(entity.GetType()).ToArray();
    }

    public object BeforeChange { get; private set; }
    public object? AfterChange { get; internal set; }
    public MutationState State { get; private set; }
    internal string StateMethodName => State switch
    {
        MutationState.Added => "InsertAsync",
        MutationState.Modified => "UpdateAsync",
        MutationState.Deleted => "DeleteAsync",
        _ => throw new ArgumentOutOfRangeException(nameof(State))
    };
    public PropertyInfo[] PrimaryKeys { get; private set; }
}