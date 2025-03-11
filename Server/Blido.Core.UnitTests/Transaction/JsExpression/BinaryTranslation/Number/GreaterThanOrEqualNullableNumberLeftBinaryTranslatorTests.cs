using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Number;

public class GreaterThanOrEqualNullableNumberLeftBinaryTranslatorTests
{
    [Fact]
    public void SupportedBinaries_ShouldNotContainNull()
    {
        // Arrange
        var operators = GreaterThanOrEqualNullableNumberLeftBinaryTranslator.SupportedBinaries;

        // Act
        var containsDefault = operators.Contains(null);

        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void TranslateBinary_AppendsNullCoalitionObjectOperatorOnLeftSide()
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
        GreaterThanOrEqualNullableNumberLeftBinaryTranslator.TranslateBinary(builder, expression, processExpression);

        // Assert
        Assert.Equal("(x??{})>=(x)", builder.ToString());
    }
}