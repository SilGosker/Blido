using System.Linq.Expressions;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation;

public class BinaryExpressionComparerTests
{
    [Fact]
    public void Equals_WhenBothAreNull_ReturnsTrue()
    {
        // Arrange
        BinaryExpressionComparer comparer = new();
        BinaryExpression? x = null;
        BinaryExpression? y = null;

        // Act
        bool result = comparer.Equals(x, y);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_WhenXIsNullAndYIsNotNull_ReturnsFalse()
    {
        // Arrange
        BinaryExpressionComparer comparer = new();
        BinaryExpression? x = null;
        BinaryExpression y = Expression.Add(Expression.Constant(1), Expression.Constant(2));

        // Act
        bool result = comparer.Equals(x, y);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WhenXIsNotNullAndYIsNull_ReturnsFalse()
    {
        // Arrange
        BinaryExpressionComparer comparer = new();
        BinaryExpression x = Expression.Add(Expression.Constant(1), Expression.Constant(2));
        BinaryExpression? y = null;

        // Act
        bool result = comparer.Equals(x, y);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WhenXAndYAreDifferentNodeTypes_ReturnsFalse()
    {
        // Arrange
        BinaryExpressionComparer comparer = new();
        BinaryExpression x = Expression.Add(Expression.Constant(1), Expression.Constant(2));
        BinaryExpression y = Expression.Subtract(Expression.Constant(1), Expression.Constant(2));
        // Act
        bool result = comparer.Equals(x, y);
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WhenLeftTypeNotEqualsRightType_ReturnsFalse()
    {
        // Arrange
        BinaryExpressionComparer comparer = new();
        BinaryExpression x = Expression.Add(Expression.Constant(1f), Expression.Constant(2f));
        BinaryExpression y = Expression.Add(Expression.Constant(1), Expression.Constant(2));

        // Act
        bool result = comparer.Equals(x, y);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WhenXAndYAreReferenceEqual_ReturnsTrue()
    {
        // Arrange
        BinaryExpressionComparer comparer = new();
        BinaryExpression x = Expression.Add(Expression.Constant(1), Expression.Constant(2));
        BinaryExpression y = x;

        // Act
        bool result = comparer.Equals(x, y);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_WhenXAndYAreEqual_ReturnsTrue()
    {
        // Arrange
        BinaryExpressionComparer comparer = new();
        BinaryExpression x = Expression.Add(Expression.Constant(1), Expression.Constant(2));
        BinaryExpression y = Expression.Add(Expression.Constant(1), Expression.Constant(2));

        // Act
        bool result = comparer.Equals(x, y);
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GetHashCode_WhenLeftTypeEqualsRight_ReturnsSameHashCode()
    {
        // Arrange
        BinaryExpressionComparer comparer = new();
        BinaryExpression x = Expression.Add(Expression.Constant(1), Expression.Constant(2));
        BinaryExpression y = Expression.Add(Expression.Constant(1), Expression.Constant(2));

        // Act
        int hashCodeX = comparer.GetHashCode(x);
        int hashCodeY = comparer.GetHashCode(y);
        
        // Assert
        Assert.Equal(hashCodeX, hashCodeY);
    }

    [Fact]
    public void GetHashCode_WhenLeftTypeNotEqualsRight_ReturnsDifferentHashCode()
    {
        // Arrange
        BinaryExpressionComparer comparer = new();
        BinaryExpression x = Expression.Add(Expression.Constant(1), Expression.Constant(2));
        BinaryExpression y = Expression.Add(Expression.Constant(1f), Expression.Constant(2f));
        // Act
        int hashCodeX = comparer.GetHashCode(x);
        int hashCodeY = comparer.GetHashCode(y);

        // Assert
        Assert.NotEqual(hashCodeX, hashCodeY);
    }

    [Fact]
    public void GetHashCode_WhenNodeTypeIsNotEqual_ReturnsDifferentHashCode()
    {
        // Arrange
        BinaryExpressionComparer comparer = new();
        BinaryExpression x = Expression.Add(Expression.Constant(1), Expression.Constant(2));
        BinaryExpression y = Expression.Subtract(Expression.Constant(1), Expression.Constant(2));

        // Act
        int hashCodeX = comparer.GetHashCode(x);
        int hashCodeY = comparer.GetHashCode(y);
        
        // Assert
        Assert.NotEqual(hashCodeX, hashCodeY);
    }
}