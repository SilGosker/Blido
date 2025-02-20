using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.UnaryTranslation.Default;

public class PostDecrementAssignUnaryTranslatorTests
{
    [Fact]
    public void SupportedUnaries_ShouldNotContainNull()
    {
        // Arrange
        var supportedUnaries = PostDecrementAssignUnaryTranslator.SupportedUnaries;
        
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
        var expression = Expression.PostDecrementAssign(Expression.Parameter(typeof(int)));
        ProcessExpression processNext = x =>
        {
            if (x is ParameterExpression)
            {
                builder.Append("array");
            }
        };
        
        // Act
        PostDecrementAssignUnaryTranslator.TranslateUnary(builder, expression, processNext);

        // Assert
        Assert.Equal("(array--)", builder.ToString());
    }
}