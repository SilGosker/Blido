using System.Linq.Expressions;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation;

public class BinaryExpressionComparer : IEqualityComparer<BinaryExpression>
{
    public bool Equals(BinaryExpression? x, BinaryExpression? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null) return false;
        if (y is null) return false;
        if (x.GetType() != y.GetType()) return false;
        
        if (x.NodeType != y.NodeType) return false;
        if (x.Left.Type != y.Left.Type) return false;
        if (x.Right.Type != y.Right.Type) return false;
        return true;
    }

    public int GetHashCode(BinaryExpression obj)
    {
        HashCode hashCode = new HashCode();
        hashCode.Add(obj.NodeType);

        hashCode.Add(obj.Left.Type);
        if (obj.Left is UnaryExpression { NodeType: ExpressionType.Convert } leftUnary)
        {
            hashCode.Add(leftUnary.Operand.Type);
        }

        hashCode.Add(obj.Right.Type);

        if (obj.Right is UnaryExpression { NodeType: ExpressionType.Convert } rightUnary)
        {
            hashCode.Add(rightUnary.Operand.Type);
        }

        return hashCode.ToHashCode();
    }
}