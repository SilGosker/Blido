using System.Linq.Expressions;

namespace Blido.Core.Transaction.JsExpression.UnaryTranslation.Default;

public class UnaryPlusUnaryTranslator : IUnaryTranslator
{
    public static TranslateUnary TranslateUnary => (builder, expression, processNext) =>
    {
        builder.Append("(+");
        processNext(expression.Operand);
        builder.Append(')');
    };

    public static UnaryExpression[] SupportedUnaries => new[]
    {
        Expression.UnaryPlus(Expression.Parameter(typeof(int))),
    };
}