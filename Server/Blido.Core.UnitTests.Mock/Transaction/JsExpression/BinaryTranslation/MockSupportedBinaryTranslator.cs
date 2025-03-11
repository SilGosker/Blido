using System.Linq.Expressions;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation;

public class MockSupportedBinaryTranslator : IBinaryTranslator
{
    public static TranslateBinary TranslateBinary => (builder, expression, processExpression) => throw new NotImplementedException();

    public static BinaryExpression[] SupportedBinaries => new[]
    {
        Expression.GreaterThanOrEqual(
            Expression.Convert(
                Expression.Constant(0L, typeof(long)), typeof(long?)),
            Expression.Constant(0L, typeof(long?)))
    };
}