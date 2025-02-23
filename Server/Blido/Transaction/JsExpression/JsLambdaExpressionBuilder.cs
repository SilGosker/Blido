using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression;

public class JsLambdaExpressionBuilder
{
    public static void AppendLambda(StringBuilder sb, LambdaExpression lambda, ProcessExpression processExpression)
    {
        sb.Append('(');
        bool first = true;
        foreach (ParameterExpression parameterExpression in lambda.Parameters)
        {
            if (!first)
            {
                sb.Append(',');
            }
            processExpression(parameterExpression);
        
            first = false;
        }

        sb.Append(")=>");
        processExpression(lambda.Body);
    }
}