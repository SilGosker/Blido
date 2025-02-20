using System.Linq.Expressions;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.UnaryTranslation.Default;

public class ArrayLengthUnaryTranslator : IUnaryTranslator
{
    public static TranslateUnary TranslateUnary => (builder, expression, processNext) =>
    {
        processNext(expression.Operand);
        builder.Append(".length");
    };

    public static UnaryExpression[] SupportedUnaries => new[]
    {
        Expression.ArrayLength(Expression.Parameter(typeof(int[])))
    };
}