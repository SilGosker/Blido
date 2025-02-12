using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Linq;

public class WhereMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processNext) =>
    {
        processNext(expression.Arguments[0]);
        sb.Append(".filter(");
        processNext(expression.Arguments[1]);
        sb.Append(')');
    };

    public static MethodInfo[] SupportedMethods => typeof(Enumerable)
        .GetMethods()
        .Where(e => e.Name == nameof(Enumerable.Where)).ToArray();
}