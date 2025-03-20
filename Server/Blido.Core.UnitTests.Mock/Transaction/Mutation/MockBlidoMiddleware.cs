namespace Blido.Core.Transaction.Mutation;

public class MockBlidoMiddleware : IBlidoMiddleware
{
    public static int TotalTimesCalled { get; set; } = 0;
    public int TimesCalled { get; private set; } = 0;
    public ValueTask ExecuteAsync(MutationContext context, ProcessNextDelegate next, CancellationToken cancellationToken)
    {
        TimesCalled++;
        TotalTimesCalled++;
        return next();
    }
}