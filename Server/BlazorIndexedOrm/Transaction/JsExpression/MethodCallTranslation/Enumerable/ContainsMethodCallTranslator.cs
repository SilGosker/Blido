using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class ContainsMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processNext) =>
    {
        processNext(expression.Arguments[0]);
        sb.Append(".contains(");
        processNext(expression.Arguments[1]);
        sb.Append(')');
    };

    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(System.Linq.Enumerable).GetMethods().First(m => m.Name == nameof(System.Linq.Enumerable.Contains)
                                                               && m.GetParameters().Length == 2)
    };
}