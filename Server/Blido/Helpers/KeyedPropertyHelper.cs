using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Blido.Core.Helpers;

public class KeyedPropertyHelper
{
    public static IEnumerable<PropertyInfo> GetKeys(Type type)
    {
        return type.GetProperties().Where(x => !Attribute.IsDefined(x, typeof(NotMappedAttribute))
                                               && (Attribute.IsDefined(x, typeof(KeyAttribute))
                                                   || x.Name.Equals("id", StringComparison.OrdinalIgnoreCase)));
    }
}