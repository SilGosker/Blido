using System.Linq.Expressions;
using Blido.Core.Helpers;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Number;

public class GreaterThanOrEqualNullableNumberBinaryTranslator : IBinaryTranslator
{
    public static TranslateBinary TranslateBinary => (builder, expression, processExpression) =>
    {
        builder.Append('(');
        processExpression(expression.Left);
        builder.Append("??window)>=(");
        processExpression(expression.Right);
        builder.Append("??window)");
    };

    public static BinaryExpression[] SupportedBinaries => 
        NumberHelper.NumberTypes.Select(type => Expression.GreaterThanOrEqual(
            Expression.Parameter(typeof(Nullable<>).MakeGenericType(type)),
            Expression.Parameter(typeof(Nullable<>).MakeGenericType(type)))).ToArray();
}