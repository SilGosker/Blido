using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Number;

public class LessThanOrEqualNullableNumberBinaryTranslatorTests
{
    [Fact]
    public void SupportedBinaries_ShouldNotContainNull()
    {
        // Arrange
        var operators = LessThanOrEqualNullableNumberBinaryTranslator.SupportedBinaries;
        
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
        var expression = Expression.LessThanOrEqual(
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
        LessThanOrEqualNullableNumberBinaryTranslator.TranslateBinary(builder, expression, processExpression);

        // Assert
        Assert.Equal("(x??window)<=(x??window)", builder.ToString());
    }
}