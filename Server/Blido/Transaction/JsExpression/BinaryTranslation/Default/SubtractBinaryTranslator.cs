using System.Linq.Expressions;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class SubtractBinaryTranslator : IBinaryTranslator
{
    public static TranslateBinary TranslateBinary => (builder, expression, processNext) =>
    {
        builder.Append('(');
        processNext(expression.Left);
        builder.Append(")-(");
        processNext(expression.Right);
        builder.Append(')');
    };

    public static BinaryExpression[] SupportedBinaries => new[]
    {
        Expression.Subtract(Expression.Constant(1), Expression.Constant(2))
    };
}