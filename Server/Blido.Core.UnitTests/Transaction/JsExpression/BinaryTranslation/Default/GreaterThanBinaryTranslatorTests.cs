using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class GreaterThanBinaryTranslatorTests
{
    [Fact]
    public void TranslateBinary_AppendsGreaterThan()
    {
        // Arrange
        var builder = new StringBuilder();
        var expression = Expression.MakeBinary(ExpressionType.GreaterThan, Expression.Constant(1), Expression.Constant(2));
        ProcessExpression processNext = x => builder.Append(x.ToString());

        // Act
        GreaterThanBinaryTranslator.TranslateBinary(builder, expression, processNext);
        
        // Assert
        Assert.Equal("(1)>(2)", builder.ToString());
    }
}