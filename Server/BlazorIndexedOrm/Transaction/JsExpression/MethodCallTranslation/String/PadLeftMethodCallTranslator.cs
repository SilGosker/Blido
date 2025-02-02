using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class PadLeftMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processExpression) =>
    {
        sb.Append(".padStart(");
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
        typeof(string).GetMethod(nameof(string.PadLeft), new[] {typeof(int), typeof(char)}),
        typeof(string).GetMethod(nameof(string.PadLeft), new[] {typeof(int)})
    };
    #nullable restore
}