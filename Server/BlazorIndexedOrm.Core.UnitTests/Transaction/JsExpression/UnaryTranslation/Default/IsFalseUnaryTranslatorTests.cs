using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.UnaryTranslation.Default;

public class IsFalseUnaryTranslatorTests
{
    [Fact]
    public void SupportedUnaries_ShouldNotContainNull()
    {
        // Arrange
        var supportedUnaries = IsFalseUnaryTranslator.SupportedUnaries;

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
        var expression = Expression.IsFalse(Expression.Constant(false));
        ProcessExpression processNext = x =>
        {
            if (x is ConstantExpression { Value: bool b })
            {
                builder.Append(b);
            }
        };
        
        // Act
        IsFalseUnaryTranslator.TranslateUnary(builder, expression, processNext);
        
        // Assert
        Assert.Equal("(!False)", builder.ToString());
    }
}