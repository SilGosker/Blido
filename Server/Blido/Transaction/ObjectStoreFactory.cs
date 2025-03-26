using Blido.Core.Options;
using Blido.Core.Transaction.JsExpression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Blido.Core.Transaction;

public class ObjectStoreFactory : IObjectStoreFactory
{
    private readonly IJSRuntime _jsRuntime;
    IJSRuntime IObjectStoreFactory.JsRuntime => _jsRuntime;
    private readonly IExpressionBuilder _expressionBuilder;
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptions<MutationConfiguration> _mutationOptions;
    public ObjectStoreFactory(IExpressionBuilder expressionBuilder, IJSRuntime jsRuntime, IServiceProvider serviceProvider, IOptions<MutationConfiguration> options)
    {
        ArgumentNullException.ThrowIfNull(expressionBuilder);
        ArgumentNullException.ThrowIfNull(jsRuntime);
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(options);
        _expressionBuilder = expressionBuilder;
        _jsRuntime = jsRuntime;
        _serviceProvider = serviceProvider;
        _mutationOptions = options;
    }

    object IObjectStoreFactory.GetObjectStore(IndexedDbContext context, Type entityType)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(entityType);

        var transactionProviderType = typeof(QueryProvider<>).MakeGenericType(entityType);
        var transactionProvider = (IQueryProvider)Activator.CreateInstance(transactionProviderType, _jsRuntime, _expressionBuilder)!;
        
        var objectStoreType = typeof(ObjectStore<>).MakeGenericType(entityType);
        var objectStore = Activator.CreateInstance(objectStoreType, context, transactionProvider, _serviceProvider, _mutationOptions)!;
        
        transactionProvider.SetObjectStore(objectStore);
        
        return objectStore;
    }
}