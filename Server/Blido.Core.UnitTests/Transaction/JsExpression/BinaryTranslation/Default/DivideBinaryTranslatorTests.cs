using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class DivideBinaryTranslatorTests
{
    [Fact]
    public void SupportedHashes_DoesNotContainDefault()
    {
        // Arrange
        var operators = DivideBinaryTranslator.SupportedBinaries;
        
        // Act
        var containsDefault = operators.Contains(null);
        
        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void TranslateBinary_AppendsDivide()
    {
        // Arrange
        var expression = Expression.MakeBinary(ExpressionType.Divide, Expression.Constant(1), Expression.Constant(2));
        var builder = new StringBuilder();
        // Act
        DivideBinaryTranslator.TranslateBinary(builder, expression, x => builder.Append(x));
        // Assert
        Assert.Equal("(1)/(2)", builder.ToString());
    }
}