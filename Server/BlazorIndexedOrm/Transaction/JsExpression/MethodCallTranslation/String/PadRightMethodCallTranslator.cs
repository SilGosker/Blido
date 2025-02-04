using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class PadRightMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processExpression) =>
    {
        processExpression(expression.Object!);
        sb.Append(".padEnd(");
        processExpression(expression.Arguments[0]);
        if (expression.Arguments.Count > 1)
        {
            sb.Append(',');
            processExpression(expression.Arguments[1]);
        }

        sb.Append(')');
    };

#nullable disable
    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(string).GetMethod(nameof(string.PadRight), new[] { typeof(int) }),
        typeof(string).GetMethod(nameof(string.PadRight), new[] { typeof(int), typeof(char) })
    };
#nullable restore
}