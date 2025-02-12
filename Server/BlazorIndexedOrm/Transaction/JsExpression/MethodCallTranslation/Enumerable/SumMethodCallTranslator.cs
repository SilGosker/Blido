using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class SumMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, expression, processNext) =>
    {
        processNext(expression.Arguments[0]);
        if (expression.Arguments.Count == 2)
        {
            builder.Append(".map(");
            processNext(expression.Arguments[1]);
            builder.Append(')');
        }
        builder.Append(".reduce((a,b)=>a+b,0)");
    };

    public static MethodInfo[] SupportedMethods => typeof(System.Linq.Enumerable).GetMethods()
        .Where(m => m.Name == nameof(System.Linq.Enumerable.Sum)).ToArray();
}