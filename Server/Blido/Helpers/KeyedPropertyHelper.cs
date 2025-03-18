using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Blido.Core.Helpers;

public class KeyedPropertyHelper
{
    public static IEnumerable<PropertyInfo> GetKeys(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return EnumerateKeys(type);
    }

    private static IEnumerable<PropertyInfo> EnumerateKeys(Type type)
    {
        bool foundKeyAttribute = false;
        PropertyInfo? idInfo = null;
        PropertyInfo? idCapsInfo = null;
        PropertyInfo? idLowerInfo = null;
        PropertyInfo? idReverseInfo = null;

        foreach (PropertyInfo x in type.GetProperties())
        {
            if (Attribute.IsDefined(x, typeof(NotMappedAttribute)))
            {
                continue;
            }
            if (Attribute.IsDefined(x, typeof(KeyAttribute)))
            {
                foundKeyAttribute = true;
                yield return x;
            }
            else
            {
                switch (x.Name)
                {
                    case "id":
                        idLowerInfo = x;
                        break;
                    case "ID":
                        idCapsInfo = x;
                        break;
                    case "Id":
                        idInfo = x;
                        break;
                    case "iD":
                        idReverseInfo = x;
                        break;
                }
            }
        }

        if (foundKeyAttribute)
        {
            yield break;
        }

        if (idInfo != null)
        {
            yield return idInfo;
        }

        if (idCapsInfo != null)
        {
            yield return idCapsInfo;
        }

        if (idLowerInfo != null)
        {
            yield return idLowerInfo;
        }

        if (idReverseInfo != null)
        {
            yield return idReverseInfo;
        }
    }
}