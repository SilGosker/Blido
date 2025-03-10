using System.Linq.Expressions;

namespace Blido.Core.Transaction.JsExpression.UnaryTranslation;

public class MockUnsupportedUnaryTranslator : IUnaryTranslator
{
    public static TranslateUnary TranslateUnary => (builder, expression, next) =>
    {
          throw new NotImplementedException();
    };

    public static UnaryExpression[] SupportedUnaries => new[]
    {
        Expression.Convert(Expression.Parameter(typeof(Exception)), typeof(NotImplementedException))
    };
}