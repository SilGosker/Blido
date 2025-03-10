using System.Linq.Expressions;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class OrElseBinaryTranslator : IBinaryTranslator
{
    public static TranslateBinary TranslateBinary => (builder, expression, processNext) =>
    {
        builder.Append('(');
        processNext(expression.Left);
        builder.Append(")||(");
        processNext(expression.Right);
        builder.Append(')');
    };

    public static BinaryExpression[] SupportedBinaries => new[]
    {
        Expression.OrElse(Expression.Constant(true), Expression.Constant(false))
    };
}