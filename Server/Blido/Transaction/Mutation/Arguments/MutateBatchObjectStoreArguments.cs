using System.Text.Json.Serialization;

namespace Blido.Core.Transaction.Mutation.Arguments;

internal class MutateBatchObjectStoreArguments
{
    [JsonPropertyName("objectStore")]
    internal string ObjectStore { get; set; } = string.Empty;

    [JsonPropertyName("entities")]
    internal List<MutateBatchEntityArguments> Entities { get; set; } = new();
}