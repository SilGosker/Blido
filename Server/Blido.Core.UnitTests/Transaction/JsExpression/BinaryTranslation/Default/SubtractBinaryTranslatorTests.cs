using System.Linq.Expressions;
using System.Text;
using Moq;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class SubtractBinaryTranslatorTests
{
    [Fact]
    public void SupportedHashes_DoesNotContainDefault()
    {
        // Arrange
        var operators = SubtractBinaryTranslator.SupportedBinaries;
        
        // Act
        var containsDefault = operators.Contains(null);
        
        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void TranslateBinary_AppendsSubtract()
    {
        // Arrange
        var builder = new StringBuilder();
        var expression = Expression.MakeBinary(ExpressionType.Subtract, Expression.Constant(1), Expression.Constant(2));
        ProcessExpression processNext = x => builder.Append(x);

        // Act
        SubtractBinaryTranslator.TranslateBinary(builder, expression, processNext);
        
        // Assert
        Assert.Equal("(1)-(2)", builder.ToString());
    }
}