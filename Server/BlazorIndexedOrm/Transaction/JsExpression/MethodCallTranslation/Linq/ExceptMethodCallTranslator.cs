using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Linq;

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
        typeof(Enumerable).GetMethods().First(e => e.Name == nameof(Enumerable.Except) && e.GetParameters().Length == 2)
    };
}