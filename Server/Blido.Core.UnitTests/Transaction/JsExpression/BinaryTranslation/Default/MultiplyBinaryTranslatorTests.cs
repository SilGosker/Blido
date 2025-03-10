using System.Linq.Expressions;
using System.Text;
using Moq;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class MultiplyBinaryTranslatorTests
{
    [Fact]
    public void SupportedHashes_DoesNotContainDefault()
    {
        // Arrange
        var operators = MultiplyBinaryTranslator.SupportedBinaries;

        // Act
        var containsDefault = operators.Contains(null);

        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void TranslateBinary_AppendsMultiply()
    {
        // Arrange
        var builder = new StringBuilder();
        var expression = Expression.MakeBinary(ExpressionType.Multiply, Expression.Constant(1), Expression.Constant(2));
        ProcessExpression processNext = next => { builder.Append(next); };

        // Act
        MultiplyBinaryTranslator.TranslateBinary(builder, expression, processNext);

        // Assert
        Assert.Equal("(1)*(2)", builder.ToString());
    }
}