using System.Linq.Expressions;
using System.Text;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MemberTranslation;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression;

public class JsMemberExpressionBuilder
{
    public static void AppendMember(StringBuilder sb, IMemberTranslatorFactory memberTranslatorFactory, MemberExpression expression, ProcessExpression processExpression)
    {
#pragma warning disable CS8600
        if (memberTranslatorFactory.TryGetValue(expression.Member, out TranslateMember translateMember))
#pragma warning restore CS8600
        {
            translateMember(sb, expression, processExpression);
            return;
        }

        string name = NameResolver.ResolveObjectFieldName(expression.Member);

        if (expression.Expression != null)
        {
            processExpression(expression.Expression);
        }

        sb.Append('.').Append(name);
    }
}