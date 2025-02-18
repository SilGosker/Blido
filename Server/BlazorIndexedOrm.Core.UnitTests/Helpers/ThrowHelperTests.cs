using System.Collections.Frozen;
using System.Linq.Expressions;
using System.Reflection;

namespace BlazorIndexedOrm.Core.Helpers;

public class ThrowHelperTests
{
    [Fact]
    public void ThrowUnsupportedException_WhenMethodInfoIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        MethodInfo methodInfo = null!;

        // Act
        Action act = () => ThrowHelper.ThrowUnsupportedException(methodInfo);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("methodInfo", exception.ParamName);
    }

    [Fact]
    public void ThrowUnsupportedException_WhenMethodInfoIsNotNull_ShouldThrowMessage()
    {
        // Arrange
        MethodInfo methodInfo = typeof(ThrowHelper).GetMethod(nameof(ThrowHelper.ThrowUnsupportedException), new Type[] { typeof(MethodInfo) })!;

        // Act
        Action act = () => ThrowHelper.ThrowUnsupportedException(methodInfo);

        // Assert
        var exception = Assert.Throws<NotSupportedException>(act);
        Assert.Equal("Using the method BlazorIndexedOrm.Core.Helpers.ThrowHelper.ThrowUnsupportedException(System.Reflection.MethodInfo) is not supported. Use a different method or overload", exception.Message);
    }

    [Fact]
    public void ThrowUnsupportedException_WhenBinaryIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        BinaryExpression binary = null!;
        // Act
        Action act = () => ThrowHelper.ThrowUnsupportedException(binary);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("binary", exception.ParamName);
    }

    [Fact]
    public void ThrowUnSupportedException_WithBinaryExpression_ShouldThrowUnsupportedException()
    {
        // Arrange
        var binary = Expression.Add(Expression.Constant(1), Expression.Constant(2));
        
        // Act
        Action act = () => ThrowHelper.ThrowUnsupportedException(binary);

        // Assert
        var exception = Assert.Throws<NotSupportedException>(act);
        Assert.Equal("Using the binary expression System.Int32.Add(System.Int32) is not supported. Use a different expression or overload", exception.Message);
    }

    [Fact]
    public void ThrowDictionaryIsNotReadonlyException_WhenDictionaryIsNotReadonly_DoesNotThrow()
    {
        // Arrange
        var expected = new Dictionary<string, string>();

        // Act
        ThrowHelper.ThrowDictionaryIsNotReadonlyException(expected, out var actual);

        // Assert
        Assert.Same(expected, actual);
    }

    [Fact]
    public void ThrowDictionaryIsNotReadonlyException_WhenDictionaryIsReadonly_ShouldThrowInvalidOperationException()
    {
        // Arrange
        IReadOnlyDictionary<string, string> readonlyDictionary = new Dictionary<string, string>().ToFrozenDictionary();

        // Act
        Action act = () => ThrowHelper.ThrowDictionaryIsNotReadonlyException(readonlyDictionary, out _);
        
        // Assert
        var exception = Assert.Throws<InvalidOperationException>(act);
        Assert.Equal("Cannot add custom translator after configuration.", exception.Message);
    }
}