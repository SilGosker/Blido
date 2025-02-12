using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Linq;

public class ElementAtOrDefaultMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processNext) =>
    {
        processNext(expression.Arguments[0]);
        sb.Append('[');
        processNext(expression.Arguments[1]);
        sb.Append(']');
    };

    public static MethodInfo[] SupportedMethods => typeof(Enumerable).GetMethods()
        .Where(m => m.Name == nameof(Enumerable.ElementAtOrDefault)).ToArray();
}