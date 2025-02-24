using System.Reflection;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class LongCountMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processNext) =>
    {
        processNext(expression.Arguments[0]);
        if (expression.Arguments.Count != 2)
        {
            sb.Append(".length");
            return;
        }
        sb.Append(".reduce((_t,_e)=>(");
        processNext(expression.Arguments[1]);
        sb.Append(")(_e)?_t+1:_t,0)");
    };

    public static MethodInfo[] SupportedMethods => typeof(System.Linq.Enumerable).GetMethods()
        .Where(m => m.Name == nameof(System.Linq.Enumerable.LongCount)).ToArray();
}