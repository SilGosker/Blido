using System.ComponentModel.DataAnnotations;

namespace Blido.Core.Transaction.Mutation.KeyGeneration.KeyGenerators;

public class EntityWithGuidKey
{
    [Key]
    public Guid? Key { get; set; }
}