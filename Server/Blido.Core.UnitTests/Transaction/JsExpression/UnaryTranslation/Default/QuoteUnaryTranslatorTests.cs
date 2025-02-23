using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.UnaryTranslation.Default;

public class QuoteUnaryTranslatorTests
{
    [Fact]
    public void SupportedUnaries_ShouldNotContainNull()
    {
        // Arrange
        var supportedUnaries = QuoteUnaryTranslator.SupportedUnaries;
        
        // Act
        var containsNull = supportedUnaries.Contains(null);
        
        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateBinary_ProcessesOperand()
    {
        // Arrange
        var builder = new StringBuilder();
        var expression = Expression.Quote(Expression.Lambda(Expression.Constant(1)));
        ProcessExpression processNext = x =>
        {
            if (x is LambdaExpression)
            {
                builder.Append("array");
            }
        };

        // Act
        QuoteUnaryTranslator.TranslateUnary(builder, expression, processNext);
        
        // Assert
        Assert.Equal("array", builder.ToString());
    }
}