using System.Linq.Expressions;

namespace Blido.Core.Transaction.JsExpression.UnaryTranslation.Default;

public class PreDecrementAssignUnaryTranslator : IUnaryTranslator
{
    public static TranslateUnary TranslateUnary => (builder, expression, processNext) =>
    {
        builder.Append("(--");
        processNext(expression.Operand);
        builder.Append(')');
    };
    public static UnaryExpression[] SupportedUnaries => new[]
    {
        Expression.PreDecrementAssign(Expression.Parameter(typeof(int)))
    };
}