using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression;

public class JsParameterExpressionBuilder
{
    public static void AppendParameter(StringBuilder stringBuilder, ParameterExpression parameter, ProcessExpression processExpression)
    {
        if (!string.IsNullOrWhiteSpace(parameter.Name))
        {
            stringBuilder.Append(parameter.Name);
        }
        else
        {
            stringBuilder.Append('x');
        }
    }
}