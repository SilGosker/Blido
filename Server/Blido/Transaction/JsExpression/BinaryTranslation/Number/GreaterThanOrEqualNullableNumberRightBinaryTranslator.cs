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
        builder.Append("??window)");
    };

    private static readonly Type[] NumberTypes = new[]
    {
        typeof(int), typeof(long), typeof(short), typeof(byte), typeof(float), typeof(double), typeof(decimal),
        typeof(uint), typeof(ulong), typeof(ushort), typeof(sbyte)
    };

    public static BinaryExpression[] SupportedBinaries => 
        NumberHelper.NumberTypes.Select(type => Expression.GreaterThanOrEqual(
            Expression.Parameter(type),
            Expression.Convert(Expression.Parameter(typeof(Nullable<>).MakeGenericType(type)), type))).ToArray();
}