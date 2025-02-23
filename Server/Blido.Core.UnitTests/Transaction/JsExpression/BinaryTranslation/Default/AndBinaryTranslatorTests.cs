using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class AndBinaryTranslatorTests
{
    [Fact]
    public void SupportedHashes_DoesNotContainDefault()
    {
        // Arrange
        var operators = AndBinaryTranslator.SupportedHashes;

        // Act
        var containsDefault = operators.Contains(default);

        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void SupportedHashes_IsPartOfCoreBinaryTranslators()
    {
        // Arrange
        var operators = AndBinaryTranslator.SupportedHashes;
        // Act
        var areAllPartOfCoreBinaryTranslator = operators.All(x => Enum.IsDefined((CoreBinaryTranslators)x.Hash));
        // Assert
        Assert.True(areAllPartOfCoreBinaryTranslator);
    }

    [Theory]
    [InlineData(ExpressionType.And)]
    public void TryMatchBinary_ReturnsTrueForAnd(ExpressionType expressionType)
    {
        // Arrange
        var binaryExpression = Expression.MakeBinary(expressionType, Expression.Constant(1), Expression.Constant(2));
        // Act
        var isMatch = AndBinaryTranslator.TryMatchBinary(binaryExpression, out var hash);
        // Assert
        Assert.True(isMatch);
        Assert.Equal((int)CoreBinaryTranslators.AndBinaryTranslator, hash.Hash);
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
    [InlineData(ExpressionType.GreaterThan)]
    [InlineData(ExpressionType.LessThan)]
    public void TryMatchBinary_ReturnsFalseForNonAnd(ExpressionType expressionType)
    {
        // Arrange
        var binaryExpression = Expression.MakeBinary(expressionType, Expression.Constant(1), Expression.Constant(2));
        
        // Act
        var isMatch = AndBinaryTranslator.TryMatchBinary(binaryExpression, out var hash);
        
        // Assert
        Assert.False(isMatch);
        Assert.Equal(default, hash);
    }

    [Fact]
    public void TranslateBinary_AppendsCorrectJs()
    {
        // Arrange
        var expression = Expression.MakeBinary(ExpressionType.And, Expression.Constant(1), Expression.Constant(2));
        var builder = new StringBuilder();

        void ProcessNext(Expression next) => builder.Append(next switch
        {
            ConstantExpression constantExpression => constantExpression.Value!.ToString(),
            _ => throw new NotSupportedException()
        });

        // Act
        AndBinaryTranslator.TranslateBinary(builder, expression, ProcessNext);
        
        // Assert
        Assert.Equal("(1)&(2)", builder.ToString());
    }
}