namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Linq;

public class CountMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processNext) =>
    {
        if (expression.Arguments.Count == -0)
        {
            sb.Append(".length");
            return;
        }

        sb.Append(".filter(");
        processNext(expression.Arguments[0]);
        sb.Append(").length");
    };
}