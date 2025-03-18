using System.ComponentModel.DataAnnotations;

namespace Blido.Core.Helpers;

public class EntityWithIdAndKey
{
    public int Id { get; set; }

    [Key]
    public string Key { get; set; }
}