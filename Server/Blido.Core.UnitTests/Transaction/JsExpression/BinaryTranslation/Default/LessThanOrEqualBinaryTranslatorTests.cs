using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class LessThanOrEqualBinaryTranslatorTests
{
    [Fact]
    public void SupportedHashes_DoesNotContainDefault()
    {
        // Arrange
        var operators = LessThanOrEqualBinaryTranslator.SupportedBinaries;

        // Act
        var containsDefault = operators.Contains(null);

        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void TranslateBinary_ReturnsCorrectJsExpression()
    {
        // Arrange
        var binaryExpression = Expression.MakeBinary(ExpressionType.LessThanOrEqual, Expression.Constant(1), Expression.Constant(2));
        StringBuilder builder = new();
        ProcessExpression processNext = next => builder.Append(next);
        
        // Act
        LessThanOrEqualBinaryTranslator.TranslateBinary(builder, binaryExpression, processNext);
        
        // Assert
        Assert.Equal("(1)<=(2)", builder.ToString());
    }
}