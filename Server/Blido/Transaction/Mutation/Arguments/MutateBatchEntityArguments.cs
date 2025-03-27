using System.Text.Json.Serialization;

namespace Blido.Core.Transaction.Mutation.Arguments;

public class MutateBatchEntityArguments
{
    internal MutateBatchEntityArguments()
    {

    }
    [JsonPropertyName("state")]
    public MutationState State { get; set; }

    [JsonPropertyName("entity")]
    public string Entity { get; set; } = string.Empty;
}