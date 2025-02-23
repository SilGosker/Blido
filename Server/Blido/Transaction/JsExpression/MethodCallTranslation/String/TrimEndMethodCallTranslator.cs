using System.Reflection;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class TrimEndMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processExpression) =>
    {
        processExpression(expression.Object!);
        sb.Append(".trimEnd()");
    };

#nullable disable
    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(string).GetMethod(nameof(string.TrimEnd), Array.Empty<Type>())!
    };
#nullable restore
}