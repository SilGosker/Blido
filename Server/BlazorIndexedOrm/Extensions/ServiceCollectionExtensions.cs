using BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorIndexedOrm.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterIndexedDbDatabase<TDatabase>(this IServiceCollection serviceCollection)
        where TDatabase : IndexedDbDatabase
    {
        serviceCollection.AddScoped<TDatabase>();
        serviceCollection.AddSingleton<IJsMethodCallTranslatorFactory, JsMethodCallTranslatorFactory>();
        return serviceCollection;
    }
}