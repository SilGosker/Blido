using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class LessThanBinaryTranslatorTests
{
    [Fact]
    public void SupportedHashes_DoesNotContainDefault()
    {
        // Arrange
        var operators = LessThanBinaryTranslator.SupportedBinaries;

        // Act
        var containsDefault = operators.Contains(null);

        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void TranslateBinary_AppendsLessThan()
    {
        // Arrange
        var expression = Expression.MakeBinary(ExpressionType.LessThan, Expression.Constant(1), Expression.Constant(2));
        StringBuilder sb = new();

        // Act
        LessThanBinaryTranslator.TranslateBinary(sb, expression, (e) => sb.Append(e));

        // Assert
        Assert.Equal("(1)<(2)", sb.ToString());
    }
}