using System.Reflection;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class ElementAtMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processNext) =>
    {
        sb.Append('(');
        processNext(expression.Arguments[0]);
        sb.Append('[');
        processNext(expression.Arguments[1]);
        sb.Append(']');
        sb.Append("??throw new Error(\"Index was out of range\"))");
    };

    public static MethodInfo[] SupportedMethods => typeof(System.Linq.Enumerable)
        .GetMethods().Where(m => m.Name == nameof(System.Linq.Enumerable.ElementAt)).ToArray();
}