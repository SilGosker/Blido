using System.Linq.Expressions;
using System.Text;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MemberTranslation;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression;

public class JsMemberExpressionBuilder
{
    public static void AppendMember(StringBuilder sb, IMemberTranslatorFactory memberTranslatorFactory,
        MemberExpression expression, ProcessExpression processExpression)
    {
#nullable disable
        if (memberTranslatorFactory.TryGetValue(expression.Member, out TranslateMember translateMember))
        {
            translateMember(sb, expression, processExpression);
            return;
        }
#nullable restore

        string name = NameResolver.ResolveObjectFieldName(expression.Member);

        if (expression.Expression != null)
        {
            processExpression(expression.Expression);
        }

        sb.Append('.').Append(name);
    }
}