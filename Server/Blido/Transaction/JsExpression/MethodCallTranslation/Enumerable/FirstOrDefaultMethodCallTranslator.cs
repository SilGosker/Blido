using System.Reflection;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class FirstOrDefaultMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processNext) =>
    {
        processNext(expression.Arguments[0]);
        if (expression.Arguments.Count == 2)
        {
            sb.Append(".find(");
            processNext(expression.Arguments[1]);
            sb.Append(')');
            return;
        }

        sb.Append("[0]");
    };

    // FirstOrDefault(IEnumerable<>)
    // FirstOrDefault(IENumerable<>, Func<,>)
    public static MethodInfo[] SupportedMethods => typeof(System.Linq.Enumerable).GetMethods().Where(e =>
        e.Name == nameof(System.Linq.Enumerable.FirstOrDefault)
        && (e.GetParameters().Length == 1 ||
            (e.GetParameters().Length == 2
             && e.GetParameters()[1].ParameterType.IsGenericType
             && e.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Func<,>))
        )).ToArray();
}