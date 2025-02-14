using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression;

public class JsUnaryExpressionBuilder
{
    public static void AppendUnary(StringBuilder sb, UnaryExpression expression, ProcessExpression processExpression)
    {

        switch (expression.NodeType)
        {
            case ExpressionType.ArrayLength:
                processExpression(expression.Operand);
                sb.Append(".length");
                return;
            case ExpressionType.Convert:
                processExpression(expression.Operand);
                return;
        }

        sb.Append('(');
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
            default:
                throw new ArgumentOutOfRangeException(nameof(expression.NodeType));
        }
        processExpression(expression.Operand);
        sb.Append(')');
    }
}