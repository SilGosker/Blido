using System.Linq.Expressions;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.UnaryTranslation.Char;

public class ConvertCharToIntUnaryTranslator : IUnaryTranslator
{
    public static TranslateUnary TranslateUnary => (builder, expression, processNext) =>
    {
        processNext(expression.Operand);
        builder.Append(".charCodeAt()");
    };

    public static UnaryExpression[] SupportedUnaries => new[]
    {
        Expression.Convert(Expression.Parameter(typeof(char)), typeof(int)),
        Expression.Convert(Expression.Parameter(typeof(char?)), typeof(int)),
    };
}