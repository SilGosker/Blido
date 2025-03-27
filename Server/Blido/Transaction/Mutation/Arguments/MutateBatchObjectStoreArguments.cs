using System.Text.Json.Serialization;

namespace Blido.Core.Transaction.Mutation.Arguments;

public class MutateBatchObjectStoreArguments
{
    internal MutateBatchObjectStoreArguments()
    {
    }
    [JsonPropertyName("objectStore")]
    public string ObjectStore { get; set; } = string.Empty;

    [JsonPropertyName("entities")]
    public List<MutateBatchEntityArguments> Entities { get; set; } = new();
}