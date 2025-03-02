using System.Text.Json.Serialization;
using System;

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
    public required string[]? ParsedExpressions { get; set; }
}
