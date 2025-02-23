using System.Reflection;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class ElementAtOrDefaultMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processNext) =>
    {
        processNext(expression.Arguments[0]);
        sb.Append('[');
        processNext(expression.Arguments[1]);
        sb.Append(']');
    };

    public static MethodInfo[] SupportedMethods => typeof(System.Linq.Enumerable).GetMethods()
        .Where(m => m.Name == nameof(System.Linq.Enumerable.ElementAtOrDefault)).ToArray();
}