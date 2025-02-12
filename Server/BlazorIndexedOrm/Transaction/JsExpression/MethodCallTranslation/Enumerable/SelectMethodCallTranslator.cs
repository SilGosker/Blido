using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class SelectMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, expression, processNext) =>
    {
        processNext(expression.Arguments[0]);
        builder.Append(".map(");
        processNext(expression.Arguments[1]);
        builder.Append(')');
    };

    public static MethodInfo[] SupportedMethods => typeof(System.Linq.Enumerable).GetMethods()
        .Where(m => m.Name == nameof(System.Linq.Enumerable.Select))
        .ToArray();
}