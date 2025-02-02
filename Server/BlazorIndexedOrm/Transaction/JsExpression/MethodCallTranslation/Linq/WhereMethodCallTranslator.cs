using System.Linq.Expressions;
using BlazorIndexedOrm.Core.Helpers;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Linq;

public class WhereMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processNext) =>
    {
        var lambda = (LambdaExpression)expression.Arguments[0];

        if (lambda.Parameters.Count != 1)
        {
            ThrowHelper.ThrowUnsupportedException(expression.Method);
        }

        sb.Append(".filter(");
        processNext(expression.Arguments[0]);
        sb.Append(')');
    };

}