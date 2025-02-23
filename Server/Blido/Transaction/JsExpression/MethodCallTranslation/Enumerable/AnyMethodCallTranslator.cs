using System.Reflection;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

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
        typeof(System.Linq.Enumerable).GetMethods().First(m => m.Name == nameof(System.Linq.Enumerable.Any)
                                                               && m.GetParameters().Length == 1)!,
        typeof(System.Linq.Enumerable).GetMethods().First(m => m.Name == nameof(System.Linq.Enumerable.Any)
                                                               && m.GetParameters().Length == 2)
    };
}