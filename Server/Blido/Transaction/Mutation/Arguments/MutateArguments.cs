using System.Text.Json;
using System.Text.Json.Serialization;

namespace Blido.Core.Transaction.Mutation.Arguments;

public class MutateArguments
{
    private MutateArguments()
    {
    }
    internal static async Task<MutateArguments> CreateAsync(MutationContext mutationContext, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(mutationContext);
        var entity = mutationContext.Entities[0].BeforeChange;
        return new MutateArguments()
        {
            Database = mutationContext.Database.Name,
            ObjectStore = NameResolver.ResolveObjectStoreName(entity.GetType()),
            Entity = JsonSerializer.Serialize(entity),
            Version = await mutationContext.Database.GetVersionAsync(cancellationToken)
        };
    }

    [JsonPropertyName("database")]
    public string Database { get; set; } = string.Empty;

    [JsonPropertyName("version")]
    public ulong Version { get; set; }

    [JsonPropertyName("objectStore")]
    public string ObjectStore { get; set; } = string.Empty;

    [JsonPropertyName("entity")]
    public string Entity { get; set; } = string.Empty;
}