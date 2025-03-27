using System.Text.Json.Serialization;

namespace Blido.Core.Transaction.Mutation.Arguments;

[JsonSerializable(typeof(MutateBatchArguments))]
internal partial class MutateBatchArgumentsSerializationContext : JsonSerializerContext;