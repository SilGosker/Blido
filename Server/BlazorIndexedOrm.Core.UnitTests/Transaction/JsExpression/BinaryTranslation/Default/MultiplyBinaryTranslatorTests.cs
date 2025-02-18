using System.Linq.Expressions;
using System.Text;
using Moq;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class MultiplyBinaryTranslatorTests
{
    [Fact]
    public void SupportedHashes_DoesNotContainDefault()
    {
        // Arrange
        var operators = MultiplyBinaryTranslator.SupportedHashes;

        // Act
        var containsDefault = operators.Contains(default);

        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void SupportedHashes_IsPartOfCoreBinaryTranslators()
    {
        // Arrange
        var operators = MultiplyBinaryTranslator.SupportedHashes;
        
        // Act
        var areAllPartOfCoreBinaryTranslator = operators.All(x => Enum.IsDefined((CoreBinaryTranslators)x.Hash));
        
        // Assert
        Assert.True(areAllPartOfCoreBinaryTranslator);
    }

    [Theory]
    [InlineData(ExpressionType.Multiply)]
    [InlineData(ExpressionType.MultiplyChecked)]
    public void TryMatchBinary_ReturnsTrueForMultiply(ExpressionType expressionType)
    {
        // Arrange
        var binaryExpression = Expression.MakeBinary(expressionType, Expression.Constant(1), Expression.Constant(2));
        
        // Act
        var isMatch = MultiplyBinaryTranslator.TryMatchBinary(binaryExpression, out var hash);
        
        // Assert
        Assert.True(isMatch);
        Assert.Equal((int)CoreBinaryTranslators.MultiplyBinaryTranslator, hash.Hash);
    }

    [Theory]
    [InlineData(ExpressionType.Or)]
    [InlineData(ExpressionType.Add)]
    [InlineData(ExpressionType.AddChecked)]
    [InlineData(ExpressionType.Subtract)]
    [InlineData(ExpressionType.SubtractChecked)]
    [InlineData(ExpressionType.Divide)]
    [InlineData(ExpressionType.Modulo)]
    [InlineData(ExpressionType.Equal)]
    [InlineData(ExpressionType.GreaterThan)]
    [InlineData(ExpressionType.GreaterThanOrEqual)]
    [InlineData(ExpressionType.LessThan)]
    [InlineData(ExpressionType.LessThanOrEqual)]
    public void TryMatchBinary_ReturnsFalseForNonMultiply(ExpressionType expressionType)
    {
        // Arrange
        var binaryExpression = Expression.MakeBinary(expressionType, Expression.Constant(1), Expression.Constant(2));
        
        // Act
        var isMatch = MultiplyBinaryTranslator.TryMatchBinary(binaryExpression, out var hash);
        
        // Assert
        Assert.False(isMatch);
        Assert.Equal(default, hash);
    }

    [Fact]
    public void TranslateBinary_AppendsMultiply()
    {
        // Arrange
        var builder = new StringBuilder();
        var expression = Expression.MakeBinary(ExpressionType.Multiply, Expression.Constant(1), Expression.Constant(2));
        ProcessExpression processNext = next => { builder.Append(next); };

        // Act
        MultiplyBinaryTranslator.TranslateBinary(builder, expression, processNext);

        // Assert
        Assert.Equal("(1)*(2)", builder.ToString());
    }
}