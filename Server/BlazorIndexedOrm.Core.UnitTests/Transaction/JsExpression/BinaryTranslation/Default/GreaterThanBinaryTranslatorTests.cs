using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class GreaterThanBinaryTranslatorTests
{
    [Fact]
    public void SupportedHashes_DoesNotContainDefault()
    {
        // Arrange
        var operators = GreaterThanBinaryTranslator.SupportedHashes;

        // Act
        var containsDefault = operators.Contains(default);

        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void SupportedHashes_IsPartOfCoreBinaryTranslators()
    {
        // Arrange
        var operators = GreaterThanBinaryTranslator.SupportedHashes;
        
        // Act
        var areAllPartOfCoreBinaryTranslator = operators.All(x => Enum.IsDefined((CoreBinaryTranslators)x.Hash));
        
        // Assert
        Assert.True(areAllPartOfCoreBinaryTranslator);
    }

    [Theory]
    [InlineData(ExpressionType.GreaterThan)]
    public void TryMatchBinary_ReturnsTrueForGreaterThan(ExpressionType expressionType)
    {
        // Arrange
        var binaryExpression = Expression.MakeBinary(expressionType, Expression.Constant(1), Expression.Constant(2));
        
        // Act
        var isMatch = GreaterThanBinaryTranslator.TryMatchBinary(binaryExpression, out var hash);
        
        // Assert
        Assert.True(isMatch);
        Assert.Equal((int)CoreBinaryTranslators.GreaterThanBinaryTranslator, hash.Hash);
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
    [InlineData(ExpressionType.LessThan)]
    public void TryMatchBinary_ReturnsFalseForNonGreaterThan(ExpressionType expressionType)
    {
        // Arrange
        var binaryExpression = Expression.MakeBinary(expressionType, Expression.Constant(1), Expression.Constant(2));
        
        // Act
        var isMatch = GreaterThanBinaryTranslator.TryMatchBinary(binaryExpression, out var hash);
        
        // Assert
        Assert.False(isMatch);
    }

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

    [Fact]
    public void TranslateBinary_AppendsCorrectJsExpression()
    {
        // Arrange
        var expression = Expression.MakeBinary(ExpressionType.GreaterThan, Expression.Constant(1), Expression.Constant(2));
        var builder = new StringBuilder();
        ProcessExpression processNext = next =>
        {
            builder.Append(next);
        };
        
        // Act
        GreaterThanBinaryTranslator.TranslateBinary(builder, expression, processNext);
        
        // Assert
        Assert.Equal("(1)>(2)", builder.ToString());
    }
}