using System.ComponentModel.DataAnnotations;

namespace Blido.Core.Helpers;

public class EntityWithKey
{
    [Key]
    public int Key { get; set; }
}