namespace Blido.Core.Transaction.Mutation;

public interface IBlidoMiddleware
{
    public ValueTask ExecuteAsync(MutationContext context, ProcessNextDelegate next, CancellationToken cancellationToken);
}