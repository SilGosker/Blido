using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression;

public class JsBinaryExpressionBuilder
{
    public static void AppendEquality(StringBuilder sb, BinaryExpression binary, ProcessExpression processExpression)
    {
        sb.Append('(');
        processExpression(binary.Left);
        sb.Append(')');

        switch (binary.NodeType)
        {
            case ExpressionType.Equal:
                sb.Append("===");
                break;
            case ExpressionType.NotEqual:
                sb.Append("!==");
                break;
            case ExpressionType.Add:
                sb.Append('+');
                break;
            case ExpressionType.Subtract:
                sb.Append('-');
                break;
            case ExpressionType.Multiply:
                sb.Append('*');
                break;
            case ExpressionType.Divide:
                sb.Append('/');
                break;
            case ExpressionType.Modulo:
                sb.Append('%');
                break;
            case ExpressionType.GreaterThan:
                sb.Append('>');
                break;
            case ExpressionType.LessThan:
                sb.Append('<');
                break;
            case ExpressionType.AndAlso:
                sb.Append("&&");
                break;
            case ExpressionType.OrElse:
                sb.Append("||");
                break;
            case ExpressionType.GreaterThanOrEqual:
                sb.Append(">=");
                break;
            case ExpressionType.LessThanOrEqual:
                sb.Append("<=");
                break;
        }

        sb.Append('(');
        processExpression(binary.Right);
        sb.Append(')');
    }
}