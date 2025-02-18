using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class OrBinaryTranslatorTests
{
    [Fact]
    public void SupportedHashes_DoesNotContainDefault()
    {
        // Arrange
        var operators = OrBinaryTranslator.SupportedHashes;

        // Act
        var containsDefault = operators.Contains(default);

        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void SupportedHashes_IsPartOfCoreBinaryTranslators()
    {
        // Arrange
        var operators = OrBinaryTranslator.SupportedHashes;
        
        // Act
        var areAllPartOfCoreBinaryTranslator = operators.All(x => Enum.IsDefined((CoreBinaryTranslators)x.Hash));
        
        // Assert
        Assert.True(areAllPartOfCoreBinaryTranslator);
    }

    [Theory]
    [InlineData(ExpressionType.Or)]
    public void TryMatchBinary_ReturnsTrueForOr(ExpressionType expressionType)
    {
        // Arrange
        var binaryExpression = Expression.MakeBinary(expressionType, Expression.Constant(1), Expression.Constant(2));
        
        // Act
        var isMatch = OrBinaryTranslator.TryMatchBinary(binaryExpression, out var hash);
        
        // Assert
        Assert.True(isMatch);
        Assert.Equal((int)CoreBinaryTranslators.OrBinaryTranslator, hash.Hash);
    }

    [Theory]
    [InlineData(ExpressionType.Add)]
    [InlineData(ExpressionType.And)]
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
    public void TryMatchBinary_ReturnsFalseForNonOr(ExpressionType expressionType)
    {
        // Arrange
        var binaryExpression = Expression.MakeBinary(expressionType, Expression.Constant(1), Expression.Constant(2));
        
        // Act
        var isMatch = OrBinaryTranslator.TryMatchBinary(binaryExpression, out var hash);
        
        // Assert
        Assert.False(isMatch);
        Assert.Equal(default, hash);
    }

    [Fact]
    public void TranslateBinary_AppendsCorrectJs()
    {
        // Arrange
        var builder = new StringBuilder();
        var expression = Expression.MakeBinary(ExpressionType.Or, Expression.Constant(1), Expression.Constant(2));
        ProcessExpression processNext = x => builder.Append(x.ToString());
        
        // Act
        OrBinaryTranslator.TranslateBinary(builder, expression, processNext);
        
        // Assert
        Assert.Equal("(1)|(2)", builder.ToString());
    }
}