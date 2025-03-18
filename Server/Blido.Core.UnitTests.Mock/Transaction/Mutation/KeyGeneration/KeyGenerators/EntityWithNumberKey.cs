using System.ComponentModel.DataAnnotations;

namespace Blido.Core.Transaction.Mutation.KeyGeneration.KeyGenerators;

public class EntityWithNumberKey
{
    [Key]
    public int? Key { get; set; }
}