using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class LessThanBinaryTranslatorTests
{
    [Fact]
    public void SupportedHashes_DoesNotContainDefault()
    {
        // Arrange
        var operators = LessThanBinaryTranslator.SupportedHashes;

        // Act
        var containsDefault = operators.Contains(default);

        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void SupportedHashes_IsPartOfCoreBinaryTranslators()
    {
        // Arrange
        var operators = LessThanBinaryTranslator.SupportedHashes;
        
        // Act
        var areAllPartOfCoreBinaryTranslator = operators.All(x => Enum.IsDefined((CoreBinaryTranslators)x.Hash));
        
        // Assert
        Assert.True(areAllPartOfCoreBinaryTranslator);
    }

    [Theory]
    [InlineData(ExpressionType.LessThan)]
    public void TryMatchBinary_ReturnsTrueForLessThan(ExpressionType expressionType)
    {
        // Arrange
        var binaryExpression = Expression.MakeBinary(expressionType, Expression.Constant(1), Expression.Constant(2));
        
        // Act
        var isMatch = LessThanBinaryTranslator.TryMatchBinary(binaryExpression, out var hash);
        
        // Assert
        Assert.True(isMatch);
        Assert.Equal((int)CoreBinaryTranslators.LessThanBinaryTranslator, hash.Hash);
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
    [InlineData(ExpressionType.LessThanOrEqual)]
    [InlineData(ExpressionType.GreaterThan)]
    [InlineData(ExpressionType.GreaterThanOrEqual)]
    public void TryMatchBinary_ReturnsFalseForNonLessThan(ExpressionType expressionType)
    {
        // Arrange
        var binaryExpression = Expression.MakeBinary(expressionType, Expression.Constant(1), Expression.Constant(2));
        
        // Act
        var isMatch = LessThanBinaryTranslator.TryMatchBinary(binaryExpression, out var hash);

        // Assert
        Assert.False(isMatch);
        Assert.Equal(default, hash);
    }

    [Fact]
    public void TranslateBinary_AppendsLessThan()
    {
        // Arrange
        var expression = Expression.MakeBinary(ExpressionType.LessThan, Expression.Constant(1), Expression.Constant(2));
        StringBuilder sb = new();

        // Act
        LessThanBinaryTranslator.TranslateBinary(sb, expression, (e) => sb.Append(e));

        // Assert
        Assert.Equal("(1)<(2)", sb.ToString());
    }
}