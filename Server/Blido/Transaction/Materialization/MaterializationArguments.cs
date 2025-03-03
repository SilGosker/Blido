using System.Text.Json.Serialization;

namespace Blido.Core.Transaction.Materialization;

public class MaterializationArguments
{
    [JsonPropertyName("database")]
    public required string Database { get; set; } = string.Empty;
    
    [JsonPropertyName("objectStore")]
    public required string ObjectStore { get; set; } = string.Empty;
    
    [JsonPropertyName("version")]
    public required ulong Version { get; set; }

    [JsonPropertyName("parsedExpressions")]
    public string[]? ParsedExpressions { get; set; }

    [JsonPropertyName("parsedSelector")]
    public string? Selector { get; set; }
}
