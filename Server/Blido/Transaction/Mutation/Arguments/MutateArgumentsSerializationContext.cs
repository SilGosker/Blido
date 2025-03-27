using System.Text.Json.Serialization;

namespace Blido.Core.Transaction.Mutation.Arguments;

[JsonSerializable(typeof(MutateArguments))]
internal partial class MutateArgumentsSerializationContext : JsonSerializerContext;