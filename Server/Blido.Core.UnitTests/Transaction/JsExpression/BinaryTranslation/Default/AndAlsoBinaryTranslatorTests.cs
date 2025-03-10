using System.Linq.Expressions;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class AndAlsoBinaryTranslatorTests
{
    [Fact]
    public void SupportedHashes_DoesNotContainDefault()
    {
        // Arrange
        var operators = AndAlsoBinaryTranslator.SupportedBinaries;

        
        // Act
        var containsDefault = operators.Contains(null);
        
        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void TranslateBinary_AppendsAndAlso()
    {
        // Arrange
        var builder = new System.Text.StringBuilder();
        var expression = Expression.MakeBinary(ExpressionType.AndAlso, Expression.Constant(true), Expression.Constant(true));
        ProcessExpression processNext = x => builder.Append(x.ToString());
        
        // Act
        AndAlsoBinaryTranslator.TranslateBinary(builder, expression, processNext);
        
        // Assert
        Assert.Equal("(True)&&(True)", builder.ToString());
    }
}