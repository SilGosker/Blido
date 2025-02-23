using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

public class AddBinaryTranslatorTests
{
    [Fact]
    public void SupportedHashes_DoesNotContainDefault()
    {
        // Arrange
        var operators = AddBinaryTranslator.SupportedHashes;
        
        // Act
        var containsDefault = operators.Contains(default);

        // Assert
        Assert.False(containsDefault);
    }

    [Fact]
    public void SupportedHashes_IsPartOfCoreBinaryTranslators()
    {
        // Arrange
        var operators = AddBinaryTranslator.SupportedHashes;
        
        // Act
        var areAllPartOfCoreBinaryTranslator = operators.All(x => Enum.IsDefined((CoreBinaryTranslators)x.Hash));

        // Assert
        Assert.True(areAllPartOfCoreBinaryTranslator);
    }

    [Theory]
    [InlineData(ExpressionType.Add)]
    [InlineData(ExpressionType.AddChecked)]
    public void TryMatchBinary_ReturnsTrueForAdd(ExpressionType expressionType)
    {
        // Arrange
        var binaryExpression = Expression.MakeBinary(expressionType, Expression.Constant(1), Expression.Constant(2));

        // Act
        var isMatch = AddBinaryTranslator.TryMatchBinary(binaryExpression, out var hash);
        
        // Assert
        Assert.True(isMatch);
        Assert.Equal((int)CoreBinaryTranslators.AddBinaryTranslator, hash.Hash);
    }

    [Theory]
    [InlineData(ExpressionType.Multiply)]
    [InlineData(ExpressionType.MultiplyChecked)]
    [InlineData(ExpressionType.Subtract)]
    [InlineData(ExpressionType.SubtractChecked)]
    [InlineData(ExpressionType.Divide)]
    [InlineData(ExpressionType.Modulo)]
    public void TryMatchBinary_ReturnsFalseForNonAdd(ExpressionType expressionType)
    {
        // Arrange
        var binaryExpression = Expression.MakeBinary(expressionType, Expression.Constant(1), Expression.Constant(2));
        
        // Act
        var isMatch = AddBinaryTranslator.TryMatchBinary(binaryExpression, out var hash);

        // Assert
        Assert.False(isMatch);
        Assert.Equal(default, hash);
    }

    [Fact]
    public void TranslateBinary_AppendsAddition()
    {
        // Arrange
        var builder = new StringBuilder();
        var expression = Expression.Add(Expression.Constant(1), Expression.Constant(2));
        ProcessExpression processExpression = (next) =>
        {
            if (next is ConstantExpression { Value: int s })
            {
                builder.Append(s);
            }
        };

        // Act
        AddBinaryTranslator.TranslateBinary(builder, expression, processExpression);
        
        // Assert
        Assert.Equal("(1)+(2)", builder.ToString());
    }
}