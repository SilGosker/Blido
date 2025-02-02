namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Linq;

public class AllMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processNext) =>
    {
        sb.Append(".every(");
        processNext(expression.Arguments[0]);
        sb.Append(')');
    };
}