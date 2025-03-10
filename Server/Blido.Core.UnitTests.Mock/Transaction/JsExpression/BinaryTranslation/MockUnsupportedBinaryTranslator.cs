using System.Linq.Expressions;
using Blido.Core.Transaction.JsExpression.MethodCallTranslation.String;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation;

public class MockUnsupportedBinaryTranslator : IBinaryTranslator
{
    public static TranslateBinary TranslateBinary => (builder, expression, processExpression) =>
    {
        builder.Append("[]+(");
        processExpression(expression.Right);
        builder.Append(')');
    };

    public static BinaryExpression[] SupportedBinaries => new[]
    {
        Expression.AddChecked(Expression.Constant(1), Expression.Constant(2))
    };
}