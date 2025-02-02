namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Linq;

public class ElementAtOrDefaultMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processNext) =>
    {
        sb.Append('[');
        processNext(expression.Arguments[0]);
        sb.Append(']');
    };
}