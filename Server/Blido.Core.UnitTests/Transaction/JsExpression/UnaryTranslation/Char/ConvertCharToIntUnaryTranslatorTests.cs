using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.UnaryTranslation.Char;

public class ConvertCharToIntUnaryTranslatorTests
{
    [Fact]
    public void SupportedUnaries_ShouldNotContainNull()
    {
        // Arrange
        var supportedUnaries = ConvertCharToIntUnaryTranslator.SupportedUnaries;
        
        // Act
        var containsNull = supportedUnaries.Contains(null);
        
        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateBinary_AppendsCharCodeAt()
    {
        // Arrange
        var builder = new StringBuilder();
        var expression = Expression.Convert(Expression.Constant('t'), typeof(int));
        ProcessExpression processNext = x =>
        {
            if (x is ConstantExpression { Value: char c })
            {
                builder.Append('\'');
                builder.Append(c);
                builder.Append('\'');
            }
        };

        // Act
        ConvertCharToIntUnaryTranslator.TranslateUnary(builder, expression, processNext);

        // Assert
        Assert.Equal("'t'.charCodeAt()", builder.ToString());
    }
}