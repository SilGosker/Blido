using System.Linq.Expressions;
using BlazorIndexedOrm.Core.Transaction.JsExpression.UnaryTranslation;

namespace BlazorIndexedOrm.Core.UnitTests.Mock.Transaction.JsExpression.UnaryTranslation;

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