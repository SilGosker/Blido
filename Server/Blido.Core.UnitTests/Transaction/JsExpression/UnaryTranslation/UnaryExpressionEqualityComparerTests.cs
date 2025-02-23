using System.Linq.Expressions;

namespace Blido.Core.Transaction.JsExpression.UnaryTranslation;

public class UnaryExpressionEqualityComparerTests
{
    [Fact]
    public void Equals_WhenBothAreNull_ReturnsTrue()
    {
        // Arrange
        UnaryExpressionEqualityComparer comparer = new();
        UnaryExpression? x = null;
        UnaryExpression? y = null;

        // Act
        bool result = comparer.Equals(x, y);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_WhenXIsNullAndYIsNotNull_ReturnsFalse()
    {
        // Arrange
        UnaryExpressionEqualityComparer comparer = new();
        UnaryExpression? x = null;
        UnaryExpression y = Expression.Convert(Expression.Constant(1), typeof(int));

        // Act
        bool result = comparer.Equals(x, y);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WhenXIsNotNullAndYIsNull_ReturnsFalse()
    {
        // Arrange
        UnaryExpressionEqualityComparer comparer = new();
        UnaryExpression x = Expression.Convert(Expression.Constant(1), typeof(int));
        UnaryExpression? y = null;

        // Act
        bool result = comparer.Equals(x, y);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WhenXAndYAreDifferentTypes_ReturnsFalse()
    {
        // Arrange
        UnaryExpressionEqualityComparer comparer = new();
        UnaryExpression x = Expression.Convert(Expression.Constant(1), typeof(int));
        UnaryExpression y = Expression.Convert(Expression.Constant(1), typeof(long));

        // Act
        bool result = comparer.Equals(x, y);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WhenXAndYAreDifferentNodeTypes_ReturnsFalse()
    {
        // Arrange
        UnaryExpressionEqualityComparer comparer = new();
        UnaryExpression x = Expression.Convert(Expression.Constant(1), typeof(int));
        UnaryExpression y = Expression.Negate(Expression.Constant(1));

        // Act
        bool result = comparer.Equals(x, y);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WhenXAndYAreConvertAndHaveDifferentTypes_ReturnsFalse()
    {
        // Arrange
        UnaryExpressionEqualityComparer comparer = new();
        UnaryExpression x = Expression.Convert(Expression.Constant(1), typeof(int));
        UnaryExpression y = Expression.Convert(Expression.Constant(1), typeof(long));

        // Act
        bool result = comparer.Equals(x, y);
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WhenXAndYAreConvertAndHaveDifferentOperandTypes_ReturnsFalse()
    {
        // Arrange
        UnaryExpressionEqualityComparer comparer = new();
        UnaryExpression x = Expression.Convert(Expression.Constant(1), typeof(int));
        UnaryExpression y = Expression.Convert(Expression.Constant(1), typeof(int?));
        // Act
        bool result = comparer.Equals(x, y);
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WhenXAndYAreConvertAndHaveSameTypes_ReturnsTrue()
    {
        // Arrange
        UnaryExpressionEqualityComparer comparer = new();
        UnaryExpression x = Expression.Convert(Expression.Constant(1), typeof(int));
        UnaryExpression y = Expression.Convert(Expression.Constant(1), typeof(int));
        // Act
        bool result = comparer.Equals(x, y);
        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(typeof(int), typeof(int?))]
    [InlineData(typeof(int), typeof(long))]
    public void GetHashCode_WhenNodeTypeIsConvertAndTypesAreDifferent_ReturnsDifferentHashCode(Type exType, Type ex2Type)
    {
        // Arrange
        UnaryExpressionEqualityComparer comparer = new();
        UnaryExpression expression = Expression.Convert(Expression.Constant(1), exType);
        UnaryExpression expression2 = Expression.Convert(Expression.Constant(1), ex2Type);
        
        // Act
        int result = comparer.GetHashCode(expression);
        int result2 = comparer.GetHashCode(expression2);
        
        // Assert
        Assert.NotEqual(result, result2);
    }

    [Fact]
    public void GetHashCode_WhenNodeTypeIsConvertAndTypesAreSame_ReturnsSameHashCode()
    {
        // Arrange
        UnaryExpressionEqualityComparer comparer = new();
        UnaryExpression expression = Expression.Convert(Expression.Constant(1), typeof(int));
        UnaryExpression expression2 = Expression.Convert(Expression.Constant(1), typeof(int));
        
        // Act
        int result = comparer.GetHashCode(expression);
        int result2 = comparer.GetHashCode(expression2);
        
        // Assert
        Assert.Equal(result, result2);
    }

    [Fact]
    public void GetHashCode_WhenNodeTypeIsNotConvert_ReturnsSameHashCode()
    {
        // Arrange
        UnaryExpressionEqualityComparer comparer = new();
        UnaryExpression expression = Expression.Negate(Expression.Constant(1));
        UnaryExpression expression2 = Expression.Negate(Expression.Constant(5));
        
        // Act
        int result = comparer.GetHashCode(expression);
        int result2 = comparer.GetHashCode(expression2);
        
        // Assert
        Assert.Equal(result, result2);
    }
}