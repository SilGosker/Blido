using System.Reflection;
using BlazorIndexedOrm.Core.Helpers;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class ReplaceMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processExpression) =>
    {
        if (expression.Arguments.Any(e => e.Type != typeof(char) && e.Type != typeof(string)))
        {
            ThrowHelper.ThrowUnsupportedException(expression.Method);
        }
        processExpression(expression.Object!);
        sb.Append(".replaceAll(");
        processExpression(expression.Arguments[0]);
        sb.Append(',');
        processExpression(expression.Arguments[1]);
        sb.Append(')');
    };

#nullable disable
    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(string).GetMethod(nameof(string.Replace), new[] { typeof(char), typeof(char) }),
        typeof(string).GetMethod(nameof(string.Replace), new[] { typeof(string), typeof(string) }),
    };
#nullable restore
}