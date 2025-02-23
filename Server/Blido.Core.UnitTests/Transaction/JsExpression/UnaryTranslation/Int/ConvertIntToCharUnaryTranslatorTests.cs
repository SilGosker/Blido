using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.UnaryTranslation.Int;

public class ConvertIntToCharUnaryTranslatorTests
{
    [Fact]
    public void SupportedUnaries_ShouldNotContainNull()
    {
        // Arrange
        UnaryExpression[] supportedUnaries = ConvertIntToCharUnaryTranslator.SupportedUnaries;

        // Act
        bool containsNull = supportedUnaries.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateUnary_ShouldAppendStringFromCharCode()
    {
        // Arrange
        StringBuilder builder = new();
        var expression = Expression.Convert(Expression.Constant(1), typeof(char));
        
        ProcessExpression processExpression = (next) =>
        {
            if (next is ConstantExpression { Value: int s })
            {
                builder.Append(s);
            }
        };

        // Act
        ConvertIntToCharUnaryTranslator.TranslateUnary(builder, expression, processExpression);
        
        // Assert
        Assert.Equal("String.fromCharCode(1)", builder.ToString());
    }
}