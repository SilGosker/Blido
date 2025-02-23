using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.UnaryTranslation.Default;

public class IsTrueUnaryTranslatorTranslatorTests
{
    [Fact]
    public void SupportedUnaries_ShouldNotContainNull()
    {
        // Arrange
        var supportedUnaries = IsTrueUnaryTranslator.SupportedUnaries;
        
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
        var expression = Expression.IsTrue(Expression.Constant(true));
        ProcessExpression processNext = x =>
        {
            if (x is ConstantExpression { Value: bool b })
            {
                builder.Append(b);
            }
        };

        // Act
        IsTrueUnaryTranslator.TranslateUnary(builder, expression, processNext);

        // Assert
        Assert.Equal("(!!True)", builder.ToString());
    }
}