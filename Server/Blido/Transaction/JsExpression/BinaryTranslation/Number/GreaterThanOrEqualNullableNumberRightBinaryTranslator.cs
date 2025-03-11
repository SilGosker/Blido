using System.Linq.Expressions;
using Blido.Core.Helpers;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Number;

public class GreaterThanOrEqualNullableNumberRightBinaryTranslator : IBinaryTranslator
{
    public static TranslateBinary TranslateBinary => (builder, expression, processExpression) =>
    {
        builder.Append('(');
        processExpression(expression.Left);
        builder.Append(")>=(");
        processExpression(expression.Right);
        builder.Append("??{})");
    };

    public static BinaryExpression[] SupportedBinaries =>
        NumberHelper.NumberTypes.Select(type => Expression.GreaterThanOrEqual(
            Expression.Convert(Expression.Parameter(type), typeof(Nullable<>).MakeGenericType(type)),
            Expression.Parameter(typeof(Nullable<>).MakeGenericType(type))
        )).ToArray();
}