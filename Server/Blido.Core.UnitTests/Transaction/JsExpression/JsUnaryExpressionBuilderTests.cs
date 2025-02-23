using System.Linq.Expressions;
using System.Text;
using Blido.Core.Transaction.JsExpression.UnaryTranslation;
using Moq;

namespace Blido.Core.Transaction.JsExpression;

public class JsUnaryExpressionBuilderTests
{
    [Fact]
    public void AppendUnary_WhenTranslatorIsNotRegistered_ThrowsNotSupportedException()
    {
        // Arrange
        var sb = new StringBuilder();
        var factory = new Mock<IUnaryTranslatorFactory>();
        var expression = Expression.Not(Expression.Constant(true));
        ProcessExpression processExpression = x => { };

        factory.Setup(f => f.TryGetValue(expression, out It.Ref<TranslateUnary>.IsAny!)).Returns(false);
        // Act
        Action act = () => JsUnaryExpressionBuilder.AppendUnary(sb, factory.Object, expression, processExpression);
        // Assert
        Assert.Throws<NotSupportedException>(act);
    }

    [Fact]
    public void AppendUnary_WenTranslatorIsRegistered_CallsTranslateUnary()
    {
        // Arrange
        var sb = new StringBuilder();
        var factory = new Mock<IUnaryTranslatorFactory>();
        var expression = Expression.Not(Expression.Constant(true));
        TranslateUnary translateUnary = (builder, _, _) =>
        {
            builder.Append("succeeded");
        };
        ProcessExpression processExpression = x =>
        {
        };
        factory.Setup(f => f.TryGetValue(expression, out translateUnary!)).Returns(true);

        // Act
        JsUnaryExpressionBuilder.AppendUnary(sb, factory.Object, expression, processExpression);

        // Assert
        Assert.Equal("succeeded", sb.ToString());
    }
}