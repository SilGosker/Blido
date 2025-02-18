using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class OrElseBinaryTranslatorTests
{
    [Fact]
    public void SupportedHashes_DoesNotContainDefault()
    {
        // Arrange
        var operators = OrElseBinaryTranslator.SupportedHashes;
        
        // Act
        var containsDefault = operators.Contains(default);
        
        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void SupportedHashes_IsPartOfCoreBinaryTranslators()
    {
        // Arrange
        var operators = OrElseBinaryTranslator.SupportedHashes;
        
        // Act
        var areAllPartOfCoreBinaryTranslator = operators.All(x => Enum.IsDefined((CoreBinaryTranslators)x.Hash));
        
        // Assert
        Assert.True(areAllPartOfCoreBinaryTranslator);
    }

    [Theory]
    [InlineData(ExpressionType.OrElse)]
    public void TryMatchBinary_ReturnsTrueForOrElse(ExpressionType expressionType)
    {
        // Arrange
        var binaryExpression = Expression.MakeBinary(expressionType, Expression.Constant(true), Expression.Constant(false));
        
        // Act
        var isMatch = OrElseBinaryTranslator.TryMatchBinary(binaryExpression, out var hash);
        
        // Assert
        Assert.True(isMatch);
        Assert.Equal((int)CoreBinaryTranslators.OrElseBinaryTranslator, hash.Hash);
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
    public void TryMatchBinary_ReturnsFalseForNonOrElse(ExpressionType expressionType)
    {
        // Arrange
        var binaryExpression = Expression.MakeBinary(expressionType, Expression.Constant(1), Expression.Constant(2));
        
        // Act
        var isMatch = OrElseBinaryTranslator.TryMatchBinary(binaryExpression, out var hash);
        
        // Assert
        Assert.False(isMatch);
        Assert.Equal(default, hash);
    }

    [Fact]
    public void TranslateBinary_AppendsOrElse()
    {
        // Arrange
        var builder = new StringBuilder();
        var expression = Expression.OrElse(Expression.Constant(true), Expression.Constant(false));
        ProcessExpression processNext = (next) => builder.Append(next.ToString().ToLower());
        
        // Act
        OrElseBinaryTranslator.TranslateBinary(builder, expression, processNext);
        
        // Assert
        Assert.Equal("(true)||(false)", builder.ToString());
    }

}