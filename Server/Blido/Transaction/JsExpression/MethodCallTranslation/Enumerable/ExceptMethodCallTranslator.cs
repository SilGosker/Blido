using System.Reflection;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class ExceptMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processNext) =>
    {
        processNext(expression.Arguments[0]);
        sb.Append(".filter(_x=>!");
        processNext(expression.Arguments[1]);
        sb.Append(".contains(_x))");
    };

    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(System.Linq.Enumerable).GetMethods().First(e => e.Name == nameof(System.Linq.Enumerable.Except) && e.GetParameters().Length == 2)
    };
}