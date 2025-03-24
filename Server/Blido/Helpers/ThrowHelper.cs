using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Blido.Core.Helpers;

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

    public static void ThrowUnsupportedException(BinaryExpression binary)
    {
        ArgumentNullException.ThrowIfNull(binary);
        StringBuilder sb = new("Using the binary expression ");
        sb.Append(binary.Left.Type);
        sb.Append('.');
        sb.Append(binary.NodeType);
        sb.Append('(');
        sb.Append(binary.Right.Type);
        sb.Append(") is not supported. Use a different expression or overload");

        throw new NotSupportedException(sb.ToString());
    }
    
    public static void ThrowUnsupportedException(UnaryExpression unary)
    {
        ArgumentNullException.ThrowIfNull(unary);
        StringBuilder sb = new("Using the unary expression ");
        if (unary.NodeType == ExpressionType.Convert)
        {
            sb.Append("Convert(");
            sb.Append(unary.Operand.Type);
            sb.Append(" to ");
            sb.Append(unary.Type);
        }
        else
        {
            sb.Append(unary.NodeType);
            sb.Append('(');
            sb.Append(unary.Operand.Type);
        }
        
        sb.Append(") is not supported. Use a different expression or overload");
        throw new NotSupportedException(sb.ToString());
    }

    public static void ThrowDictionaryIsNotReadonlyException<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> readonlyDictionary,
        out Dictionary<TKey, TValue> dictionary)
        where TKey : notnull
    {
        if (readonlyDictionary is not Dictionary<TKey, TValue> tempDictionary)
        {
            throw new InvalidOperationException("Cannot add custom translator after configuration.");
        }
        dictionary = tempDictionary;
    }

    public static void ThrowTypeNotInObjectStores(Type entityType, IndexedDbContext context)
    {
        if (!context.ObjectStoreTypes.Contains(entityType))
        {
            throw new InvalidOperationException($"The entity of type {entityType} is not present in any of the objectstores in context {context.GetType()}");
        }
    }
}