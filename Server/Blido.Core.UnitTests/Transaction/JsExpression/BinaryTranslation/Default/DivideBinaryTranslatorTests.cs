using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class DivideBinaryTranslatorTests
{
    [Fact]
    public void SupportedHashes_DoesNotContainDefault()
    {
        // Arrange
        var operators = DivideBinaryTranslator.SupportedHashes;
        
        // Act
        var containsDefault = operators.Contains(default);
        
        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void SupportedHashes_IsPartOfCoreBinaryTranslators()
    {
        // Arrange
        var operators = DivideBinaryTranslator.SupportedHashes;
        
        // Act
        var areAllPartOfCoreBinaryTranslator = operators.All(x => Enum.IsDefined((CoreBinaryTranslators)x.Hash));
        
        // Assert
        Assert.True(areAllPartOfCoreBinaryTranslator);
    }

    [Theory]
    [InlineData(ExpressionType.Divide)]
    public void TryMatchBinary_ReturnsTrueForDivide(ExpressionType expressionType)
    {
        // Arrange
        var binaryExpression = Expression.MakeBinary(expressionType, Expression.Constant(1), Expression.Constant(2));
        
        // Act
        var isMatch = DivideBinaryTranslator.TryMatchBinary(binaryExpression, out var hash);

        // Assert
        Assert.True(isMatch);
        Assert.Equal((int)CoreBinaryTranslators.DivideBinaryTranslator, hash.Hash);
    }

    [Theory]
    [InlineData(ExpressionType.Or)]
    [InlineData(ExpressionType.Add)]
    [InlineData(ExpressionType.AddChecked)]
    [InlineData(ExpressionType.Multiply)]
    [InlineData(ExpressionType.MultiplyChecked)]
    [InlineData(ExpressionType.Subtract)]
    [InlineData(ExpressionType.SubtractChecked)]
    [InlineData(ExpressionType.Modulo)]
    [InlineData(ExpressionType.GreaterThan)]
    [InlineData(ExpressionType.LessThan)]
    public void TryMatchBinary_ReturnsFalseForNonDivide(ExpressionType expressionType)
    {
        // Arrange
        var binaryExpression = Expression.MakeBinary(expressionType, Expression.Constant(1), Expression.Constant(2));
        
        // Act
        var isMatch = DivideBinaryTranslator.TryMatchBinary(binaryExpression, out var hash);

        // Assert
        Assert.False(isMatch);
        Assert.Equal(default, hash);
    }

    [Fact]
    public void TranslateBinary_AppendsDivide()
    {
        // Arrange
        var expression = Expression.MakeBinary(ExpressionType.Divide, Expression.Constant(1), Expression.Constant(2));
        var builder = new StringBuilder();
        // Act
        DivideBinaryTranslator.TranslateBinary(builder, expression, x => builder.Append(x));
        // Assert
        Assert.Equal("(1)/(2)", builder.ToString());
    }
}