namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation;

public class MethodInfoEqualityComparerTests
{
    [Fact]
    public void Equals_WhenBothAreNull_ReturnsTrue()
    {
        // Arrange
        var sut = new MethodInfoEqualityComparer();
        
        // Act
        var result = sut.Equals(null, null);
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_WhenOneIsNull_ReturnsFalse()
    {
        // Arrange
        var sut = new MethodInfoEqualityComparer();
        var method = typeof(string).GetMethod(nameof(string.Contains), new [] { typeof(char) })!;
        // Act
        var result = sut.Equals(null, method);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void Equals_WithDifferentMethods_ReturnsFalse()
    {
        // Arrange
        var sut = new MethodInfoEqualityComparer();
        var left = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(char) });
        var right = typeof(string).GetMethod(nameof(string.StartsWith), new [] { typeof(char) });

        // Act
        var result = sut.Equals(left, right);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void Equals_WithSameMethods_ReturnsTrue()
    {
        // Arrange
        var sut = new MethodInfoEqualityComparer();
        var left = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(char)});
        var right = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(char) });

        // Act
        var result = sut.Equals(left, right);
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_WithDifferentGenericMethods_ReturnsFalse()
    {
        // Arrange
        var sut = new MethodInfoEqualityComparer();
        var left = typeof(List<int>).GetMethod(nameof(List<int>.Add));
        var right = typeof(List<int>).GetMethod(nameof(List<int>.Remove));
        // Act
        var result = sut.Equals(left, right);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WithEmptyGenericSameMethod_ReturnsTrue()
    {
        // Arrange
        var sut = new MethodInfoEqualityComparer();
        var left = typeof(List<>).GetMethod(nameof(List<int>.Add))!;
        var right = typeof(List<>).GetMethod(nameof(List<int>.Add))!;
        // Act
        var result = sut.Equals(left, right);
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_WithEmptyGenericAndNonEmpty_ReturnsTrue()
    {
        // Arrange
        var sut = new MethodInfoEqualityComparer();
        var left = typeof(System.Linq.Enumerable).GetMethods()
            .First().MakeGenericMethod(typeof(int));
        var right = typeof(System.Linq.Enumerable).GetMethods().First();
        
        // Act
        var result = sut.Equals(left, right);
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GetHashCode_WhenBothAreEqual_ReturnsSameValue()
    {
        // Arrange
        var sut = new MethodInfoEqualityComparer();
       
        // Act
        var result1 = sut.GetHashCode(typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(char) })!);
        var result2 = sut.GetHashCode(typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(char) })!);
       
        // Assert
        Assert.Equal(result1, result2);
    }

    [Fact]
    public void GetHashCode_WhenBothAreDifferent_ReturnsDifferentValue()
    {
        // Arrange
        var sut = new MethodInfoEqualityComparer();
        
        // Act
        var result1 = sut.GetHashCode(typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(char) })!);
        var result2 = sut.GetHashCode(typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(char) })!);
        
        // Assert
        Assert.NotEqual(result1, result2);
    }

}