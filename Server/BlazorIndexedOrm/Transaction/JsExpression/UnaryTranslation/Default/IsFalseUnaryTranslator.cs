using System.Linq.Expressions;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.UnaryTranslation.Default;

public class IsFalseUnaryTranslator : IUnaryTranslator
{
    public static TranslateUnary TranslateUnary => (builder, expression, processNext) =>
    {
        builder.Append("(!");
        processNext(expression.Operand);
        builder.Append(')');
    };

    public static UnaryExpression[] SupportedUnaries => new UnaryExpression[]
    {
        Expression.IsFalse(Expression.Parameter(typeof(bool))),
        Expression.Not(Expression.Parameter(typeof(bool)))
    };
}