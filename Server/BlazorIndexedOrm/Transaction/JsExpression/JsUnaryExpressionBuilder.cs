using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression;

public class JsUnaryExpressionBuilder
{
    public static void AppendUnary(StringBuilder sb, UnaryExpression expression, ProcessExpression processExpression)
    {
        switch (expression.NodeType)
        {
            case ExpressionType.Not:
                sb.Append('!');
                break;
            case ExpressionType.Negate:
                sb.Append('-');
                break;
            case ExpressionType.OnesComplement:
                sb.Append('~');
                break;
            case ExpressionType.ArrayLength:
                processExpression(expression.Operand);
                sb.Append(".length");
                return;
            default:
                throw new ArgumentOutOfRangeException(nameof(expression.NodeType));
        }
        sb.Append('(');
        processExpression(expression.Operand);
        sb.Append(')');
    }
}