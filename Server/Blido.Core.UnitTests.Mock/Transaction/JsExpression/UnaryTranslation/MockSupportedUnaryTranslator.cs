using System.Linq.Expressions;

namespace Blido.Core.Transaction.JsExpression.UnaryTranslation;

public class MockSupportedUnaryTranslator : IUnaryTranslator
{
    public static TranslateUnary TranslateUnary => (stringBuilder, expression, processNext) =>
    {
        processNext(expression.Operand);
        stringBuilder.Append("TEST");
    };

    public static UnaryExpression[] SupportedUnaries => new[]
    {
        Expression.UnaryPlus(Expression.Parameter(typeof(int)))
    };
}