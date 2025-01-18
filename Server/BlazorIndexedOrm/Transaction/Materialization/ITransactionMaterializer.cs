namespace BlazorIndexedOrm.Core.Transaction.Materialization;

public interface ITransactionMaterializer<TResult>
{
    public Task<TResult> ExecuteAsync(CancellationToken cancellationToken = default);
}