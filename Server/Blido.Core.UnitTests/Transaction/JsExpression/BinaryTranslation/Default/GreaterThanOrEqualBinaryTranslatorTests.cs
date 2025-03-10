using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class GreaterThanOrEqualBinaryTranslatorTests
{
    [Fact]
    public void SupportedHashes_DoesNotContainDefault()
    {
        // Arrange
        var operators = GreaterThanOrEqualBinaryTranslator.SupportedBinaries;
        
        // Act
        var containsDefault = operators.Contains(null);
        
        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void TranslateBinary_AppendsGreaterThanOrEqual()
    {
        // Arrange
        var expression = Expression.MakeBinary(ExpressionType.GreaterThanOrEqual, Expression.Constant(1), Expression.Constant(2));
        var builder = new StringBuilder();
        
        // Act
        GreaterThanOrEqualBinaryTranslator.TranslateBinary(builder, expression, x => builder.Append(x));
        
        // Assert
        Assert.Equal("(1)>=(2)", builder.ToString());
    }
}