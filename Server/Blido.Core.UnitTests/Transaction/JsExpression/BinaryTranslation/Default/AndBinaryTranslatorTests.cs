using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class AndBinaryTranslatorTests
{
    [Fact]
    public void SupportedHashes_DoesNotContainDefault()
    {
        // Arrange
        var operators = AndBinaryTranslator.SupportedBinaries;

        // Act
        var containsDefault = operators.Contains(null);

        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void TranslateBinary_AppendsAndOperator()
    {
        // Arrange
        var expression = Expression.MakeBinary(ExpressionType.And, Expression.Constant(1), Expression.Constant(2));
        var builder = new StringBuilder();

        void ProcessNext(Expression next) => builder.Append(next switch
        {
            ConstantExpression constantExpression => constantExpression.Value!.ToString(),
            _ => throw new NotSupportedException()
        });

        // Act
        AndBinaryTranslator.TranslateBinary(builder, expression, ProcessNext);
        
        // Assert
        Assert.Equal("(1)&(2)", builder.ToString());
    }
}