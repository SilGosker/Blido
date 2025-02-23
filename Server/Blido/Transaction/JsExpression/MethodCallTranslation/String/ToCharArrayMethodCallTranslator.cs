using System.Reflection;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class ToCharArrayMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processExpression) =>
    {
        processExpression(expression.Object!);
        sb.Append(".split('')");
    };

#nullable disable
    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(string).GetMethod(nameof(string.ToCharArray), Array.Empty<Type>())!
    };
#nullable restore
}