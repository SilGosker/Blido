using System.Linq.Expressions;
using BlazorIndexedOrm.Core.Helpers;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Linq;

public class FirstOrDefaultMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processNext) =>
    {
        if (expression.Arguments.Any(e => e is not LambdaExpression lambda))
        {
            ThrowHelper.ThrowUnsupportedException(expression.Method);
        }

        if (expression.Arguments.Count == 1)
        {
            sb.Append(".find(");
            processNext(expression.Arguments[0]);
            sb.Append(')');
        }
        else
        {
            sb.Append("[0]");
        }
    };
}