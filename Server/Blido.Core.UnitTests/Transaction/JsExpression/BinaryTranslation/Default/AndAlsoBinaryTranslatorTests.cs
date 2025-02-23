using System.Linq.Expressions;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class AndAlsoBinaryTranslatorTests
{
    [Fact]
    public void SupportedHashes_DoesNotContainDefault()
    {
        // Arrange
        var operators = AndAlsoBinaryTranslator.SupportedHashes;

        
        // Act
        var containsDefault = operators.Contains(default);
        
        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void SupportedHashes_IsPartOfCoreBinaryTranslators()
    {
        // Arrange
        var operators = AndAlsoBinaryTranslator.SupportedHashes;
        
        // Act
        var areAllPartOfCoreBinaryTranslator = operators.All(x => Enum.IsDefined((CoreBinaryTranslators)x.Hash));
        
        // Assert
        Assert.True(areAllPartOfCoreBinaryTranslator);
    }

    [Theory]
    [InlineData(ExpressionType.AndAlso)]
    public void TryMatchBinary_ReturnsTrueForAndAlso(ExpressionType expressionType)
    {
        // Arrange
        var binaryExpression = Expression.MakeBinary(expressionType, Expression.Constant(true), Expression.Constant(true));
        
        // Act
        var isMatch = AndAlsoBinaryTranslator.TryMatchBinary(binaryExpression, out var hash);
        
        // Assert
        Assert.True(isMatch);
        Assert.Equal((int)CoreBinaryTranslators.AndAlsoBinaryTranslator, hash.Hash);
    }

    [Theory]
    [InlineData(ExpressionType.OrElse)]
    [InlineData(ExpressionType.And)]
    [InlineData(ExpressionType.Or)]
    public void TryMatchBinary_ReturnsFalseForNonAndAlso(ExpressionType expressionType)
    {
        // Arrange
        var binaryExpression = Expression.MakeBinary(expressionType, Expression.Constant(true), Expression.Constant(false));

        // Act
        var isMatch = AndAlsoBinaryTranslator.TryMatchBinary(binaryExpression, out var hash);

        // Assert
        Assert.False(isMatch);
        Assert.Equal(default, hash);
    }

    [Fact]
    public void TranslateBinary_AppendsAndAlso()
    {
        // Arrange
        var builder = new System.Text.StringBuilder();
        var expression = Expression.MakeBinary(ExpressionType.AndAlso, Expression.Constant(true), Expression.Constant(true));
        ProcessExpression processNext = x => builder.Append(x.ToString());
        
        // Act
        AndAlsoBinaryTranslator.TranslateBinary(builder, expression, processNext);
        
        // Assert
        Assert.Equal("(True)&&(True)", builder.ToString());
    }
}