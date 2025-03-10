using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class OrBinaryTranslatorTests
{
    [Fact]
    public void SupportedHashes_DoesNotContainDefault()
    {
        // Arrange
        var operators = OrBinaryTranslator.SupportedBinaries;

        // Act
        var containsDefault = operators.Contains(null);

        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void TranslateBinary_AppendsCorrectJs()
    {
        // Arrange
        var builder = new StringBuilder();
        var expression = Expression.MakeBinary(ExpressionType.Or, Expression.Constant(1), Expression.Constant(2));
        ProcessExpression processNext = x => builder.Append(x.ToString());
        
        // Act
        OrBinaryTranslator.TranslateBinary(builder, expression, processNext);
        
        // Assert
        Assert.Equal("(1)|(2)", builder.ToString());
    }
}