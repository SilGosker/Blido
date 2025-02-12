using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Linq;

public class CountMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processNext) =>
    {
        processNext(expression.Arguments[0]);
        if (expression.Arguments.Count != 2)
        {
            sb.Append(".length");
            return;
        }

        sb.Append(".filter(");
        processNext(expression.Arguments[1]);
        sb.Append(").length");
    };

    public static MethodInfo[] SupportedMethods => typeof(Enumerable).GetMethods()
        .Where(m => m.Name == nameof(Enumerable.Count)).ToArray();
}