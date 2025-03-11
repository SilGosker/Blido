using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Number;

public class LessThanOrEqualNullableRightBinaryTranslatorTests
{
    [Fact]
    public void SupportedBinaries_ShouldNotContainNull()
    {
        // Arrange
        var operators = LessThanOrEqualNullableRightBinaryTranslator.SupportedBinaries;

        // Act
        var containsDefault = operators.Contains(null);

        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void TranslateBinary_AppendsNullCoalitionWindowOperatorsOnRightSide()
    {
        // Arrange
        var builder = new StringBuilder();
        var expression = Expression.LessThanOrEqual(
            Expression.Convert(Expression.Parameter(typeof(int)), typeof(int?)),
            Expression.Parameter(typeof(int?)));
        ProcessExpression processExpression = (next) =>
        {
                builder.Append('x');
        };
        
        // Act
        LessThanOrEqualNullableRightBinaryTranslator.TranslateBinary(builder, expression, processExpression); 
        
        // Assert
        Assert.Equal("(x)<=(x??{})", builder.ToString());
    }
}