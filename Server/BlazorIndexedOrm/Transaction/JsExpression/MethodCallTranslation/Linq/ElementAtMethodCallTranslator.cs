using BlazorIndexedOrm.Core.Helpers;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Linq;

public class ElementAtMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processNext) =>
    {
        if (expression.Object is null)
        {
            ThrowHelper.ThrowUnsupportedException(expression.Method);
            return;
        }

        sb.Append('(');
        processNext(expression.Object);
        sb.Append('[');
        processNext(expression.Arguments[0]);
        sb.Append(']');
        sb.Append("?? throw new Error('Index out of range')");
    };
}