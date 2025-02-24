using System.Reflection;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class DistinctMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, expression, processNext) =>
    {
        processNext(expression.Arguments[0]);
        builder.Append(".filter((_v, _i, _a)=>_a.indexOf(_v)===_i)");
    };

    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(System.Linq.Enumerable)
            .GetMethods()
            .First(m => m.Name == nameof(System.Linq.Enumerable.Distinct) && m.GetParameters().Length == 1)
    };
}