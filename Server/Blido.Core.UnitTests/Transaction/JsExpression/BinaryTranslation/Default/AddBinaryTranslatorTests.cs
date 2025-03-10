using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class AddBinaryTranslatorTests
{
    [Fact]
    public void SupportedHashes_DoesNotContainDefault()
    {
        // Arrange
        var operators = AddBinaryTranslator.SupportedBinaries;
        
        // Act
        var containsDefault = operators.Contains(null);

        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void TranslateBinary_AppendsAddOperator()
    {
        // Arrange
        var builder = new StringBuilder();
        var expression = Expression.Add(Expression.Constant(1), Expression.Constant(2));
        ProcessExpression processExpression = (next) =>
        {
            if (next is ConstantExpression { Value: int s })
            {
                builder.Append(s);
            }
        };

        // Act
        AddBinaryTranslator.TranslateBinary(builder, expression, processExpression);
        
        // Assert
        Assert.Equal("(1)+(2)", builder.ToString());
    }
}