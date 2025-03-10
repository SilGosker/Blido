using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class OrElseBinaryTranslatorTests
{
    [Fact]
    public void SupportedHashes_DoesNotContainDefault()
    {
        // Arrange
        var operators = OrElseBinaryTranslator.SupportedBinaries;
        
        // Act
        var containsDefault = operators.Contains(null);
        
        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void TranslateBinary_AppendsOrElse()
    {
        // Arrange
        var builder = new StringBuilder();
        var expression = Expression.OrElse(Expression.Constant(true), Expression.Constant(false));
        ProcessExpression processNext = (next) => builder.Append(next.ToString().ToLower());
        
        // Act
        OrElseBinaryTranslator.TranslateBinary(builder, expression, processNext);
        
        // Assert
        Assert.Equal("(true)||(false)", builder.ToString());
    }

}