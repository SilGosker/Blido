namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Linq;

public class ExceptMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processNext) =>
    {
        sb.Append(".filter((_x) => ");
        processNext(expression.Arguments[0]);
        sb.Append(".every((_y) => _x !== _y))");
    };
}