using BlazorIndexedOrm.Core.Transaction;
using BlazorIndexedOrm.Core.Transaction.JsExpression;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MemberTranslation;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorIndexedOrm.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterIndexedDbDatabase<TDatabase>(this IServiceCollection serviceCollection)
        where TDatabase : IndexedDbDatabase
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        serviceCollection.AddScoped<TDatabase>();
        serviceCollection.AddSingleton<IMethodCallTranslatorFactory, JsMethodCallTranslatorFactory>();
        serviceCollection.AddSingleton<IMemberTranslatorFactory, JsMemberTranslatorFactory>();
        serviceCollection.AddScoped<IExpressionBuilder, JsExpressionBuilder>();
        serviceCollection.AddScoped<IIndexedDbTransactionProviderFactory, IndexedDbTransactionProviderFactory>();

        return serviceCollection;
    }
}