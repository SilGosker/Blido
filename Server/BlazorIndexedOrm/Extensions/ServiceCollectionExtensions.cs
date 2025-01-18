using Microsoft.Extensions.DependencyInjection;

namespace BlazorIndexedOrm.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterIndexedDbDatabase<TDatabase>(this IServiceCollection serviceCollection) where TDatabase : IndexedDbDatabase
    {
        serviceCollection.AddScoped<TDatabase>();
        serviceCollection.AddScoped<IndexedDbDatabase>(sc => sc.GetRequiredService<TDatabase>());
        return serviceCollection;
    }
}