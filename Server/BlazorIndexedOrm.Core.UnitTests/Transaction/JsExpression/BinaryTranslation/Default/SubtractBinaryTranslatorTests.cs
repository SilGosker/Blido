using System.Linq.Expressions;
using System.Text;
using Moq;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class SubtractBinaryTranslatorTests
{
    [Fact]
    public void SupportedHashes_DoesNotContainDefault()
    {
        // Arrange
        var operators = SubtractBinaryTranslator.SupportedHashes;
        
        // Act
        var containsDefault = operators.Contains(default);
        
        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void SupportedHashes_IsPartOfCoreBinaryTranslators()
    {
        // Arrange
        var operators = SubtractBinaryTranslator.SupportedHashes;
        
        // Act
        var areAllPartOfCoreBinaryTranslator = operators.All(x => Enum.IsDefined((CoreBinaryTranslators)x.Hash));
        
        // Assert
        Assert.True(areAllPartOfCoreBinaryTranslator);
    }

    [Theory]
    [InlineData(ExpressionType.Subtract)]
    public void TryMatchBinary_ReturnsTrueForSubtract(ExpressionType expressionType)
    {
        // Arrange
        var binaryExpression = Expression.MakeBinary(expressionType, Expression.Constant(1), Expression.Constant(2));
        
        // Act
        var isMatch = SubtractBinaryTranslator.TryMatchBinary(binaryExpression, out var hash);
        
        // Assert
        Assert.True(isMatch);
        Assert.Equal((int)CoreBinaryTranslators.SubtractBinaryTranslator, hash.Hash);
    }

    [Theory]
    [InlineData(ExpressionType.Add)]
    [InlineData(ExpressionType.And)]
    [InlineData(ExpressionType.AddChecked)]
    [InlineData(ExpressionType.Multiply)]
    [InlineData(ExpressionType.MultiplyChecked)]
    [InlineData(ExpressionType.Or)]
    [InlineData(ExpressionType.Divide)]
    [InlineData(ExpressionType.Modulo)]
    [InlineData(ExpressionType.Equal)]
    [InlineData(ExpressionType.GreaterThan)]
    public void TryMatchBinary_ReturnsFalseForNonSubtract(ExpressionType expressionType)
    {
        // Arrange
        var binaryExpression = Expression.MakeBinary(expressionType, Expression.Constant(1), Expression.Constant(2));
        
        // Act
        var isMatch = SubtractBinaryTranslator.TryMatchBinary(binaryExpression, out var hash);
        
        // Assert
        Assert.False(isMatch);
        Assert.Equal(default, hash);
    }

    [Fact]
    public void TranslateBinary_AppendsSubtract()
    {
        // Arrange
        var builder = new StringBuilder();
        var expression = Expression.MakeBinary(ExpressionType.Subtract, Expression.Constant(1), Expression.Constant(2));
        ProcessExpression processNext = x => builder.Append(x);

        // Act
        SubtractBinaryTranslator.TranslateBinary(builder, expression, processNext);
        
        // Assert
        Assert.Equal("(1)-(2)", builder.ToString());
    }
}