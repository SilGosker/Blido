using System.Linq.Expressions;
using Blido.Core.Helpers;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Number;

public class LessThanOrEqualNullableNumberLeftBinaryTranslator : IBinaryTranslator
{
    public static TranslateBinary TranslateBinary => (builder, expression, processNext) =>
    {
        builder.Append('(');
        processNext(expression.Left);
        builder.Append("??window)<=(");
        processNext(expression.Right);
        builder.Append(')');
    };

    public static BinaryExpression[] SupportedBinaries =>
        NumberHelper.NumberTypes.Select(type =>
            Expression.LessThanOrEqual(
                Expression.Convert(
                    Expression.Parameter(typeof(Nullable<>).MakeGenericType(type)), type),
                Expression.Parameter(type))
        ).ToArray();
}