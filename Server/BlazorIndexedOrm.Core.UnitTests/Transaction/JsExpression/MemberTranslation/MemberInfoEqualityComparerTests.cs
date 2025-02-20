using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MemberTranslation;

public class MemberInfoEqualityComparerTests
{
    [Fact]
    public void Equals_WhenBothAreNull_ReturnsTrue()
    {
        // Arrange
        MemberInfoEqualityComparer comparer = new();
        MemberInfo? x = null;
        MemberInfo? y = null;
        
        // Act
        bool result = comparer.Equals(x, y);
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_WhenXIsNull_ReturnsFalse()
    {
        // Arrange
        MemberInfoEqualityComparer comparer = new();
        MemberInfo? x = null;
        MemberInfo y = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(char) })!;
        
        // Act
        bool result = comparer.Equals(x, y);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WhenYIsNull_ReturnsFalse()
    {
        // Arrange
        MemberInfoEqualityComparer comparer = new();
        MemberInfo x = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(char) })!;
        MemberInfo? y = null;
        
        // Act
        bool result = comparer.Equals(x, y);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WithDifferentTypes_ReturnsFalse()
    {
        // Arrange
        MemberInfoEqualityComparer comparer = new();
        MemberInfo x = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(char) })!;
        MemberInfo y = typeof(string).GetProperty(nameof(string.Length))!;
        
        // Act
        bool result = comparer.Equals(x, y);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WithSameType_ReturnsTrue()
    {
        // Arrange
        MemberInfoEqualityComparer comparer = new();
        MemberInfo x = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(char) })!;
        MemberInfo y = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(char) })!;
        
        // Act
        bool result = comparer.Equals(x, y);
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GetHashCode_WithAnonymousType_ReturnsHashCode()
    {
        // Arrange
        var obj1 = new { y = 1 };
        var obj2 = new { x = 1 };
        var member1 = obj1.GetType().GetMember("y")[0];
        var member2 = obj2.GetType().GetMember("x")[0];
        MemberInfoEqualityComparer comparer = new();

        // Act
        int result1 = comparer.GetHashCode(member1);
        int result2 = comparer.GetHashCode(member2);

        // Assert
        Assert.Equal(result1, result2);
    }

    [Fact]
    public void GetHashCode_WithNonAnonymousType_ReturnsHashCode()
    {
        // Arrange
        MemberInfoEqualityComparer comparer = new();
        MemberInfo obj = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(char) })!;
        
        // Act
        int result = comparer.GetHashCode(obj);
        
        // Assert
        Assert.Equal(obj.GetHashCode(), result);
    }

    [Fact]
    public void IsAnonymousType_WhenTypeHasCompilerGeneratedAttribute_ReturnsTrue()
    {
        // Arrange
        var obj = new { };
        MemberInfoEqualityComparer comparer = new();
        Type type = obj.GetType();

        // Act
        bool result = comparer.IsAnonymousType(type);
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsAnonymousType_WhenTypeNameContainsAnonymousType_ReturnsTrue()
    {
        // Arrange
        var obj = new { };
        MemberInfoEqualityComparer comparer = new();
        Type type = obj.GetType();
        
        // Act
        bool result = comparer.IsAnonymousType(type);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsAnonymousType_WithNoNAnonymousType_ReturnsFalse()
    {
        // Arrange
        MemberInfoEqualityComparer comparer = new();
        Type type = typeof(string);
        
        // Act
        bool result = comparer.IsAnonymousType(type);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsAnonymousType_WIthNullType_ThrowsArgumentNullException()
    {
        // Arrange
        MemberInfoEqualityComparer comparer = new();
        Type type = null!;

        // Act
        Action act = () => comparer.IsAnonymousType(type);

        // Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("type", exception.ParamName);
    }

    [Fact]
    public void IsAnonymousType_NullFullName_ReturnsFalse()
    {
        // Arrange
        MemberInfoEqualityComparer comparer = new();
        Type type = typeof(string);

        // Act
        bool result = comparer.IsAnonymousType(type);
        
        // Assert
        Assert.False(result);
    }
}