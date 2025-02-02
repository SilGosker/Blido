using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class ReplaceLineEndingsMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processExpression) =>
    {
        sb.Append(@".replace(/\r\n|\r|\n/g, ");
        if (expression.Arguments.Count > 0)
        {
            processExpression(expression.Arguments[0]);
        }
        else
        {
            sb.Append("'\\n'");
        }
        sb.Append("')");
    };

    #nullable disable
    public static MethodInfo[] SupportedMethods => new []
    {
        typeof(string).GetMethod(nameof(string.ReplaceLineEndings), Array.Empty<Type>()),
        typeof(string).GetMethod(nameof(string.ReplaceLineEndings), new[] { typeof(string) }),
    };
    #nullable restore
}