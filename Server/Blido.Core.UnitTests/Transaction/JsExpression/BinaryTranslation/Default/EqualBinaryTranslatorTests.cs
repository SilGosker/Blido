using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class EqualBinaryTranslatorTests
{
    [Fact]
    public void SupportedHashes_DoesNotContainDefault()
    {
        // Arrange
        var operators = EqualBinaryTranslator.SupportedBinaries;

        // Act
        var containsDefault = operators.Contains(null);
        
        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void TranslateBinary_AppendsCorrectJsExpression()
    {
        // Arrange
        var expression = Expression.Equal(Expression.Constant(1), Expression.Constant(2));
        var builder = new StringBuilder();
        ProcessExpression processNext = next =>
        {
            if (next is ConstantExpression constantExpression)
            {
                builder.Append(constantExpression.Value);
            }
        };

        // Act
        EqualBinaryTranslator.TranslateBinary(builder, expression, processNext);
        
        // Assert
        Assert.Equal("(1)===(2)", builder.ToString());
    }
}