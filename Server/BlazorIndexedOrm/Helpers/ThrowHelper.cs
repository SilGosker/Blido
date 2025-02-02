using System.Reflection;
using System.Text;

namespace BlazorIndexedOrm.Core.Helpers;

public class ThrowHelper
{
    public static void ThrowUnsupportedException(MethodInfo methodInfo)
    {
        ArgumentNullException.ThrowIfNull(methodInfo);
        StringBuilder sb = new("Using the method ");
        if (methodInfo.DeclaringType != null)
        {
            sb.Append(methodInfo.DeclaringType.FullName);
        }
        sb.Append('.');
        sb.Append(methodInfo.Name);
        sb.Append('(');
        ParameterInfo[] parameters = methodInfo.GetParameters();
        for (int i = 0; i < parameters.Length; i++)
        {
            if (i > 0)
            {
                sb.Append(", ");
            }
            sb.Append(parameters[i].ParameterType.FullName);
        }
        sb.Append(") is not supported. Use a different method or overload");

        throw new NotSupportedException(sb.ToString());
    }
}