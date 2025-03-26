using Blido.Core.Options;
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
    public static IServiceCollection RegisterIndexedDbDatabase<TDatabase>(this IServiceCollection serviceCollection,
        Action<BlidoConfiguration>? configure = null)
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

        serviceCollection.ConfigureBlido(configure);

        return serviceCollection;
    }

    internal static void ConfigureBlido(this IServiceCollection serviceCollection, Action<BlidoConfiguration>? configure)
    {
        var blidoConfiguration = new BlidoConfiguration();
        configure?.Invoke(blidoConfiguration);

        serviceCollection.Configure<BlidoConfiguration>(options => configure?.Invoke(options));
        serviceCollection.Configure(blidoConfiguration.MutationPipelineConfiguration);
    }
}