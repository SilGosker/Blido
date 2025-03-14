using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Number;

public class LessThanOrEqualNullableNumberLeftBinaryTranslatorTests
{
    [Fact]
    public void SupportedBinaries_ShouldNotContainNull()
    {
        // Arrange
        var operators = LessThanOrEqualNullableNumberLeftBinaryTranslator.SupportedBinaries;

        // Act
        var containsDefault = operators.Contains(null);

        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void TranslateBinary_AppendsNullCoalitionWindowOperatorsOnLeftSide()
    {
        // Arrange
        var builder = new StringBuilder();
        var expression = Expression.LessThanOrEqual(
            Expression.Parameter(typeof(int?)),
            Expression.Convert(Expression.Parameter(typeof(int)), typeof(int?)));
        ProcessExpression processExpression = (next) =>
        {
            builder.Append('x');
        };

        // Act
        LessThanOrEqualNullableNumberLeftBinaryTranslator.TranslateBinary(builder, expression, processExpression);
        
        // Assert
        Assert.Equal("(x??{})<=(x)", builder.ToString());
    }
}