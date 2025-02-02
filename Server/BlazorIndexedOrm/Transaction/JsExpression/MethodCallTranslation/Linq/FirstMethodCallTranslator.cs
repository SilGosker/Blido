namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Linq;

public class FirstMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processNext) =>
    {
        sb.Append('(');
        if (expression.Object is not null)
        {
            processNext(expression.Object);
        }
        // First(Func<T, bool>)
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

        sb.Append("?? throw new Error('Sequence contains no (matching) elements')");
        sb.Append(')');
    };
}