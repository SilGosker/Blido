using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.UnaryTranslation.Default;

public class UnaryPlusUnaryTranslatorTests
{
    [Fact]
    public void SupportedUnaries_ShouldNotContainNull()
    {
        // Arrange
        var supportedUnaries = UnaryPlusUnaryTranslator.SupportedUnaries;
        
        // Act
        var containsNull = supportedUnaries.Contains(null);
        
        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateBinary_AppendsLength()
    {
        // Arrange
        var builder = new StringBuilder();
        var expression = Expression.UnaryPlus(Expression.Parameter(typeof(int)));
        ProcessExpression processNext = x =>
        {
            if (x is ParameterExpression)
            {
                builder.Append("array");
            }
        };
        
        // Act
        UnaryPlusUnaryTranslator.TranslateUnary(builder, expression, processNext);
        
        // Assert
        Assert.Equal("(+array)", builder.ToString());
    }
}