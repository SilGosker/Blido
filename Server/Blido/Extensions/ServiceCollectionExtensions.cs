using Blido.Core.Transaction;
using Blido.Core.Transaction.JsExpression;
using Blido.Core.Transaction.JsExpression.BinaryTranslation;
using Blido.Core.Transaction.JsExpression.MemberTranslation;
using Blido.Core.Transaction.JsExpression.MethodCallTranslation;
using Blido.Core.Transaction.JsExpression.UnaryTranslation;
using Microsoft.Extensions.DependencyInjection;

namespace Blido.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterIndexedDbDatabase<TDatabase>(this IServiceCollection serviceCollection)
        where TDatabase : IndexedDbDatabase
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        serviceCollection.AddScoped<TDatabase>();
        serviceCollection.AddSingleton<IMethodCallTranslatorFactory, JsMethodCallTranslatorFactory>();
        serviceCollection.AddSingleton<IMemberTranslatorFactory, JsMemberTranslatorFactory>();
        serviceCollection.AddSingleton<IBinaryTranslatorFactory, JsBinaryTranslatorFactory>();
        serviceCollection.AddSingleton<IUnaryTranslatorFactory, JsUnaryTranslatorFactory>();
        serviceCollection.AddScoped<IExpressionBuilder, JsExpressionBuilder>();
        serviceCollection.AddScoped<IIndexedDbTransactionProviderFactory, IndexedDbTransactionProviderFactory>();

        return serviceCollection;
    }
}