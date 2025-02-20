using System.Linq.Expressions;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.UnaryTranslation.Int;

public class ConvertIntToCharUnaryTranslator : IUnaryTranslator
{
    public static TranslateUnary TranslateUnary => (builder, expression, processNext) =>
    {
        builder.Append("String.fromCharCode(");
        processNext(expression.Operand);
        builder.Append(')');
    };

    public static UnaryExpression[] SupportedUnaries => new[]
    {
        Expression.Convert(Expression.Constant(1), typeof(char)),
    };
}