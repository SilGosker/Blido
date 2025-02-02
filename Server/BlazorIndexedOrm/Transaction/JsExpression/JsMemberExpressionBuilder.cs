using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression;

public class JsMemberExpressionBuilder
{
    public static void AppendMember(StringBuilder sb, MemberExpression expression, ProcessExpression processExpression)
    {
        string name = NameResolver.ResolveObjectFieldName(expression.Member);

        if (expression.Expression != null)
        {
            processExpression(expression.Expression);
        }

        sb.Append('.').Append(name);
    }
}