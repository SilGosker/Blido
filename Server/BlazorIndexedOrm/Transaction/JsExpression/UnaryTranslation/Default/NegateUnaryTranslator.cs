using System.Linq.Expressions;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.UnaryTranslation.Default;

public class NegateUnaryTranslator : IUnaryTranslator
{
    public static TranslateUnary TranslateUnary => (builder, expression, processNext) =>
    {
        builder.Append("(-");
        processNext(expression.Operand);
        builder.Append(')');
    };

    public static UnaryExpression[] SupportedUnaries => new[]
    {
        Expression.Negate(Expression.Parameter(typeof(int))),
        Expression.NegateChecked(Expression.Parameter(typeof(int))),
    };
}