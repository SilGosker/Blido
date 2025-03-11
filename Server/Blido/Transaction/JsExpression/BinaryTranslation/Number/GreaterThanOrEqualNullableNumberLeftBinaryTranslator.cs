using Blido.Core.Helpers;
using System.Linq.Expressions;
using System.Numerics;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Number;

public class GreaterThanOrEqualNullableNumberLeftBinaryTranslator : IBinaryTranslator
{
    public static TranslateBinary TranslateBinary => (builder, expression, processNext) =>
    {
        builder.Append('(');
        processNext(expression.Left);
        builder.Append("??window)>=(");
        processNext(expression.Right);
        builder.Append(')');
    };

    public static BinaryExpression[] SupportedBinaries => NumberHelper.NumberTypes.Select(type => Expression.GreaterThanOrEqual(
        Expression.Parameter(typeof(Nullable<>).MakeGenericType(type)),
        Expression.Convert(Expression.Parameter(type), typeof(Nullable<>).MakeGenericType(type))
    )).ToArray();
}