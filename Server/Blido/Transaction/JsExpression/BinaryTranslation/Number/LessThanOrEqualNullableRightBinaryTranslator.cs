using System.Linq.Expressions;
using Blido.Core.Helpers;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Number;

public class LessThanOrEqualNullableRightBinaryTranslator : IBinaryTranslator
{
    public static TranslateBinary TranslateBinary => (builder, expression, processNext) =>
    {
        builder.Append('(');
        processNext(expression.Left);
        builder.Append(")<=(");
        processNext(expression.Right);
        builder.Append("??{})");
    };

    public static BinaryExpression[] SupportedBinaries => NumberHelper.NumberTypes.Select(type =>
        Expression.LessThanOrEqual(
            Expression.Convert(Expression.Parameter(type), typeof(Nullable<>).MakeGenericType(type)),
            Expression.Parameter(typeof(Nullable<>).MakeGenericType(type))
        )).ToArray();
}