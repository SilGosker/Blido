using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class NotEqualBinaryTranslatorTests
{
    [Fact]
    public void SupportedHashes_DoesNotContainDefault()
    {
        // Arrange
        var operators = NotEqualBinaryTranslator.SupportedHashes;
        
        // Act
        var containsDefault = operators.Contains(default);
        
        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void SupportedHashes_IsPartOfCoreBinaryTranslators()
    {
        // Arrange
        var operators = NotEqualBinaryTranslator.SupportedHashes;
        
        // Act
        var areAllPartOfCoreBinaryTranslator = operators.All(x => Enum.IsDefined((CoreBinaryTranslators)x.Hash));
        
        // Assert
        Assert.True(areAllPartOfCoreBinaryTranslator);
    }

    [Theory]
    [InlineData(ExpressionType.NotEqual)]
    public void TryMatchBinary_ReturnsTrueForNotEqual(ExpressionType expressionType)
    {
        // Arrange
        var binaryExpression = Expression.MakeBinary(expressionType, Expression.Constant(1), Expression.Constant(2));
        
        // Act
        var isMatch = NotEqualBinaryTranslator.TryMatchBinary(binaryExpression, out var hash);
        
        // Assert
        Assert.True(isMatch);
        Assert.Equal((int)CoreBinaryTranslators.NotEqualBinaryTranslator, hash.Hash);
    }

    [Theory]
    [InlineData(ExpressionType.Or)]
    [InlineData(ExpressionType.Add)]
    [InlineData(ExpressionType.AddChecked)]
    [InlineData(ExpressionType.Multiply)]
    [InlineData(ExpressionType.MultiplyChecked)]
    [InlineData(ExpressionType.Subtract)]
    [InlineData(ExpressionType.SubtractChecked)]
    [InlineData(ExpressionType.Divide)]
    [InlineData(ExpressionType.Modulo)]
    [InlineData(ExpressionType.Equal)]
    [InlineData(ExpressionType.GreaterThan)]
    [InlineData(ExpressionType.GreaterThanOrEqual)]
    [InlineData(ExpressionType.LessThan)]
    [InlineData(ExpressionType.LessThanOrEqual)]
    public void TryMatchBinary_ReturnsFalseForNonNotEqual(ExpressionType expressionType)
    {
        // Arrange
        var binaryExpression = Expression.MakeBinary(expressionType, Expression.Constant(1), Expression.Constant(2));
        
        // Act
        var isMatch = NotEqualBinaryTranslator.TryMatchBinary(binaryExpression, out var hash);
        
        // Assert
        Assert.False(isMatch);
        Assert.Equal(default, hash);
    }

    [Fact]

    public void TranslateBinary_AppendsCorrectJsExpression()
    {
        // Arrange
        var expression = Expression.MakeBinary(ExpressionType.Equal, Expression.Constant(1), Expression.Constant(2));
        var builder = new StringBuilder();
        ProcessExpression processNext = next =>
        {
            if (next is ConstantExpression constantExpression)
            {
                builder.Append(constantExpression.Value);
            }
        };

        // Act
        NotEqualBinaryTranslator.TranslateBinary(builder, expression, processNext);

        // Assert
        Assert.Equal("(1)!==(2)", builder.ToString());
    }

}