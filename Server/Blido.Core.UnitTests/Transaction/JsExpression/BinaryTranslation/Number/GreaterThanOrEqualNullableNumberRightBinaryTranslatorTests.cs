using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Number;

public class GreaterThanOrEqualNullableNumberRightBinaryTranslatorTests
{
    [Fact]
    public void SupportedBinaries_ShouldNotContainNull()
    {
        // Arrange
        var operators = GreaterThanOrEqualNullableNumberRightBinaryTranslator.SupportedBinaries;
        
        // Act
        var containsDefault = operators.Contains(null);
        
        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void TranslateBinary_AppendsNullCoalitionObjectOperatorsOnRightSide()
    {
        // Arrange
        var builder = new StringBuilder();
        var expression = Expression.GreaterThanOrEqual(
            Expression.Convert(Expression.Parameter(typeof(int)), typeof(int?)),
            Expression.Parameter(typeof(int?)));

        ProcessExpression processExpression = (next) =>
        {
            builder.Append('x');
        };

        // Act
        GreaterThanOrEqualNullableNumberRightBinaryTranslator.TranslateBinary(builder, expression, processExpression);

        // Assert
        Assert.Equal("(x)>=(x??{})", builder.ToString());
    }
}