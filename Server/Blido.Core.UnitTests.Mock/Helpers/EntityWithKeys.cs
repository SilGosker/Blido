using System.ComponentModel.DataAnnotations;

namespace Blido.Core.Helpers;

public class EntityWithKeys
{
    [Key]
    public int Key1 { get; set; }

    [Key]
    public int Key2 { get; set; }
}