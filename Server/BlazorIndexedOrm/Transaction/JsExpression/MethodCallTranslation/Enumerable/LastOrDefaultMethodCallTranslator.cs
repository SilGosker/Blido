using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class LastOrDefaultMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (builder, expression, processNext) =>
    {
        processNext(expression.Arguments[0]);
        if (expression.Arguments.Count == 1)
        {
            builder.Append(".at(-1)");
            return;
        }

        builder.Append(".findLast(");
        processNext(expression.Arguments[1]);
        builder.Append(')');
    };

    // LastOrDefault(IEnumerable<>)
    // LastOrDefault(IEnumerable<>, Func<,>)
    public static MethodInfo[] SupportedMethods => typeof(System.Linq.Enumerable).GetMethods().Where(e =>
        e.Name == nameof(System.Linq.Enumerable.LastOrDefault)
        && (e.GetParameters().Length == 1 ||
            (e.GetParameters().Length == 2
             && e.GetParameters()[1].ParameterType.IsGenericType
             && e.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Func<,>))
        )).ToArray();
}