using System.Linq.Expressions;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.UnaryTranslation.Default;

public class QuoteUnaryTranslator : IUnaryTranslator
{
    public static TranslateUnary TranslateUnary => (builder, expression, processNext) =>
    {
        processNext(expression.Operand);
    };

    public static UnaryExpression[] SupportedUnaries => new[]
    {
        Expression.Quote(Expression.Lambda(Expression.Constant(0)))
    };
}