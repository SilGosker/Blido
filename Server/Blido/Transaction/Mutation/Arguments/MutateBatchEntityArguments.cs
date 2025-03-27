using System.Text.Json.Serialization;

namespace Blido.Core.Transaction.Mutation.Arguments;

internal class MutateBatchEntityArguments
{
    [JsonPropertyName("state")]
    internal MutationState State { get; set; }

    [JsonPropertyName("entity")]
    internal string Entity { get; set; } = string.Empty;
}