using Blido.Core.Transaction;
using Blido.Core.Transaction.JsExpression;
using Blido.Core.Transaction.JsExpression.BinaryTranslation;
using Blido.Core.Transaction.JsExpression.MemberTranslation;
using Blido.Core.Transaction.JsExpression.MethodCallTranslation;
using Blido.Core.Transaction.JsExpression.UnaryTranslation;
using Blido.Core.Transaction.Mutation;
using Blido.Core.Transaction.Mutation.KeyGeneration;
using Microsoft.Extensions.DependencyInjection;

namespace Blido.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterIndexedDbDatabase<TDatabase>(this IServiceCollection serviceCollection)
        where TDatabase : IndexedDbContext
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        serviceCollection.AddSingleton<IMethodCallTranslatorFactory, JsMethodCallTranslatorFactory>();
        serviceCollection.AddSingleton<IMemberTranslatorFactory, JsMemberTranslatorFactory>();
        serviceCollection.AddSingleton<IBinaryTranslatorFactory, JsBinaryTranslatorFactory>();
        serviceCollection.AddSingleton<IUnaryTranslatorFactory, JsUnaryTranslatorFactory>();
        serviceCollection.AddSingleton<IKeyGeneratorFactory, KeyGeneratorFactory>();
        serviceCollection.AddSingleton<KeyGeneratorMiddleware>();

        serviceCollection.AddScoped<TDatabase>();
        serviceCollection.AddScoped<MutateMiddleware>();
        serviceCollection.AddScoped<IExpressionBuilder, JsExpressionBuilder>();
        serviceCollection.AddScoped<IObjectStoreFactory, ObjectStoreFactory>();

        return serviceCollection;
    }
}