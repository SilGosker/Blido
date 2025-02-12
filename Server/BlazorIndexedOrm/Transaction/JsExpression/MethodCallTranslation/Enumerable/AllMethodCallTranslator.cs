using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class AllMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processNext) =>
    {
        processNext(expression.Arguments[0]);
        sb.Append(".every(");
        processNext(expression.Arguments[1]);
        sb.Append(')');
    };

    public static MethodInfo[] SupportedMethods => new []
    {
        typeof(System.Linq.Enumerable).GetMethod(nameof(System.Linq.Enumerable.All))!

    };
}