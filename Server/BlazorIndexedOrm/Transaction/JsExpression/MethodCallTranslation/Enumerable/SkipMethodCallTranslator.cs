using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class SkipMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, expression, processNext) =>
    {
        processNext(expression.Arguments[0]);
        builder.Append(".slice(");
        processNext(expression.Arguments[1]);
        builder.Append(')');
    };

    public static MethodInfo[] SupportedMethods =>
        typeof(System.Linq.Enumerable).GetMethods().Where(e => e.Name == nameof(System.Linq.Enumerable.Skip)).ToArray();
}