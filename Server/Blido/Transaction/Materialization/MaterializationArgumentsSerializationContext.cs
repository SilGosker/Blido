using System.Text.Json.Serialization;

namespace Blido.Core.Transaction.Materialization;


[JsonSerializable(typeof(MaterializationArguments))]
internal partial class MaterializationArgumentsSerializationContext : JsonSerializerContext;