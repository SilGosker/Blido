using System.Linq.Expressions;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.UnaryTranslation;

public class UnaryExpressionEqualityComparer : IEqualityComparer<UnaryExpression>
{
    public bool Equals(UnaryExpression? x, UnaryExpression? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null) return false;
        if (y is null) return false;
        if (x.GetType() != y.GetType()) return false;
        if (x.NodeType != y.NodeType) return false;
        if (x.NodeType == ExpressionType.Convert)
        {
            if (x.Type != y.Type) return false;
            if (x.Operand.Type != y.Operand.Type) return false;
        }

        return true;
    }

    public int GetHashCode(UnaryExpression obj)
    {
        HashCode hash = new();
        hash.Add(obj.NodeType);

        if (obj.NodeType == ExpressionType.Convert)
        {
            hash.Add(obj.Type);
            hash.Add(obj.Operand.Type);
        }
        
        return hash.ToHashCode();
    }
}