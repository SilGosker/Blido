using System.Linq.Expressions;

namespace Blido.Core.Helpers;

public class CaseInsensitivityHelperTests
{
    [Theory]
    [InlineData(StringComparison.CurrentCultureIgnoreCase)]
    [InlineData(StringComparison.InvariantCultureIgnoreCase)]
    [InlineData(StringComparison.OrdinalIgnoreCase)]
    public void IsCaseSensitive_WithIgnoreCaseComparison_ShouldReturnTrue(StringComparison stringComparison)
    {
        // Act
        var result = CaseInsensitivityHelper.IsCaseInsensitive(stringComparison);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(StringComparison.CurrentCulture)]
    [InlineData(StringComparison.InvariantCulture)]
    [InlineData(StringComparison.Ordinal)]
    public void IsCaseSensitive_WithCaseSensitiveComparison_ShouldReturnFalse(StringComparison stringComparison)
    {
        // Act
        var result = CaseInsensitivityHelper.IsCaseInsensitive(stringComparison);
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryGetStringComparison_WithNullExpression_ReturnsFalse()
    {
        // Arrange
        Expression expression = null!;

        // Act
        var result = CaseInsensitivityHelper.TryGetStringComparison(expression, out StringComparison stringComparison);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryGetStringComparison_WithNonConstantExpression_ReturnsFalse()
    {
        // Arrange
        Expression expression = Expression.Add(Expression.Constant(10), Expression.Constant(20));

        // Act
        var result = CaseInsensitivityHelper.TryGetStringComparison(expression, out StringComparison stringComparison);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryGetStringComparison_WithInvalidConstantExpression_ReturnsFalse()
    {
        // Arrange
        Expression expression = Expression.Constant("test");

        // Act
        var result = CaseInsensitivityHelper.TryGetStringComparison(expression, out StringComparison stringComparison);
        
        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData((StringComparison)int.MaxValue)]
    public void TryGetStringComparison_WithUnDefinedConstantExpression_ReturnsFalse(StringComparison stringComparison)
    {
        // Arrange
        Expression expression = Expression.Constant(stringComparison);

        // Act
        var result = CaseInsensitivityHelper.TryGetStringComparison(expression, out StringComparison resultStringComparison);

        Assert.False(result);
    }

    [Theory]
    [InlineData(StringComparison.CurrentCultureIgnoreCase)]
    [InlineData(StringComparison.InvariantCultureIgnoreCase)]
    [InlineData(StringComparison.OrdinalIgnoreCase)]
    [InlineData(StringComparison.CurrentCulture)]
    [InlineData(StringComparison.InvariantCulture)]
    [InlineData(StringComparison.Ordinal)]
    public void TryGetStringComparison_WithValidConstantExpression_ReturnsTrue(StringComparison stringComparison)
    {
        // Arrange
        Expression expression = Expression.Constant(stringComparison);

        // Act
        var result = CaseInsensitivityHelper.TryGetStringComparison(expression, out StringComparison resultStringComparison);

        Assert.True(result);
    }

    [Theory]
    [InlineData(StringComparison.CurrentCultureIgnoreCase)]
    [InlineData(StringComparison.InvariantCultureIgnoreCase)]
    [InlineData(StringComparison.OrdinalIgnoreCase)]
    public void IsCaseInsensitive_WithIgnoreCaseComparison_ShouldReturnTrue(StringComparison stringComparison)
    {
        // Arrange
        Expression expression = Expression.Constant(stringComparison);
        
        // Act
        var result = CaseInsensitivityHelper.IsCaseInsensitive(expression);
        
        // Assert
        Assert.True(result);
    }
}