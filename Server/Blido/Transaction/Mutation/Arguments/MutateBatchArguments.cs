using System.Text.Json;
using System.Text.Json.Serialization;

namespace Blido.Core.Transaction.Mutation.Arguments;

public class MutateBatchArguments
{
    [JsonPropertyName("database")]
    public string Database { get; set; } = string.Empty;

    [JsonPropertyName("version")]
    public ulong Version { get; set; }

    [JsonPropertyName("objectStores")]
    public List<MutateBatchObjectStoreArguments> ObjectStores { get; set; } = new();

    internal static async Task<MutateBatchArguments> CreateAsync(MutationContext context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);
        MutateBatchArguments arguments = new()
        {
            Database = context.Database.Name,
            Version = await context.Database.GetVersionAsync(cancellationToken)
        };

        foreach (var entityContexts in context.Entities.GroupBy(x => x.BeforeChange.GetType()))
        {
            var objectStoreContext = new MutateBatchObjectStoreArguments()
            {
                ObjectStore = NameResolver.ResolveObjectStoreName(entityContexts.Key)
            };

            foreach (MutationEntityContext entityContext in entityContexts)
            {
                objectStoreContext.Entities.Add(new MutateBatchEntityArguments()
                {
                    Entity = JsonSerializer.Serialize(entityContext.BeforeChange),
                    State = entityContext.State
                });
            }

            arguments.ObjectStores.Add(objectStoreContext);
        }

        return arguments;
    }
}