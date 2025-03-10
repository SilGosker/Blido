using System.Linq.Expressions;
using Blido.Core.Helpers;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Number;

public class LessThanOrEqualNullableNumberBinaryTranslator : IBinaryTranslator
{
    public static TranslateBinary TranslateBinary => (builder, expression, processNext) =>
    {
        builder.Append('(');
        processNext(expression.Left);
        builder.Append("??window)<=(");
        processNext(expression.Right);
        builder.Append("??window)");
    };

    public static BinaryExpression[] SupportedBinaries => 
        NumberHelper.NumberTypes.Select(type => Expression.LessThanOrEqual(
            Expression.Parameter(typeof(Nullable<>).MakeGenericType(type)),
            Expression.Parameter(typeof(Nullable<>).MakeGenericType(type)))).ToArray();
}