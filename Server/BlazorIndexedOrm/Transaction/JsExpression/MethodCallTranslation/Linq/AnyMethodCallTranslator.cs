using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Linq;

public class AnyMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processNext) =>
    {
        processNext(expression.Arguments[0]);
        sb.Append(".some(");
        processNext(expression.Arguments[1]);
        sb.Append(')');
    };

    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(Enumerable).GetMethods().First(m => m.Name == nameof(Enumerable.Any)
                                                   && m.GetParameters().Length == 1)!,
        typeof(Enumerable).GetMethods().First(m => m.Name == nameof(Enumerable.Any)
                                                   && m.GetParameters().Length == 2)
    };
}