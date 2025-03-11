using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Number;

public class GreaterThanOrEqualNullableNumberBinaryTranslatorTests
{
    [Fact]
    public void SupportedBinaries_ShouldNotContainNull()
    {
        // Arrange
        var operators = GreaterThanOrEqualNullableNumberBinaryTranslator.SupportedBinaries;

        // Act
        var containsDefault = operators.Contains(null);

        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void TranslateBinary_AppendsNullCoalitionWindowOperatorsOnBothSides()
    {
        // Arrange
        var builder = new StringBuilder();
        var expression = Expression.GreaterThanOrEqual(
            Expression.Parameter(typeof(int?)),
            Expression.Parameter(typeof(int?)));

        ProcessExpression processExpression = (next) =>
        {
            if (next is ParameterExpression)
            {
                builder.Append('x');
            }
        };

        // Act
        GreaterThanOrEqualNullableNumberBinaryTranslator.TranslateBinary(builder, expression, processExpression);
        
        // Assert
        Assert.Equal("(x??window)>=(x??window)", builder.ToString());
    }
}