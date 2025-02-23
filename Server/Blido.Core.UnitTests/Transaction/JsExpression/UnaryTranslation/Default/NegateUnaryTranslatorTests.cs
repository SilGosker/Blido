using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.UnaryTranslation.Default;

public class NegateUnaryTranslatorTests
{
    [Fact]
    public void SupportedUnaries_ShouldNotContainNull()
    {
        // Arrange
        var supportedUnaries = NegateUnaryTranslator.SupportedUnaries;

        // Act
        var containsNull = supportedUnaries.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateBinary_AppendsNot()
    {
        // Arrange
        var builder = new StringBuilder();
        var expression = Expression.Negate(Expression.Constant(1));
        ProcessExpression processNext = x =>
        {
            if (x is ConstantExpression { Value: int b })
            {
                builder.Append(b);
            }
        };
        
        // Act
        NegateUnaryTranslator.TranslateUnary(builder, expression, processNext);
        
        // Assert
        Assert.Equal("(-1)", builder.ToString());
    }
}