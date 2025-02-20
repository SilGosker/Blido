using System.Linq.Expressions;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.UnaryTranslation.Default;

public class IsTrueUnaryTranslator : IUnaryTranslator
{
    public static TranslateUnary TranslateUnary => (builder, expression, processNext) =>
    {
        builder.Append("(!!");
        processNext(expression.Operand);
        builder.Append(')');
    };

    public static UnaryExpression[] SupportedUnaries => new[]
    {
        Expression.IsTrue(Expression.Parameter(typeof(bool)))
    };
}