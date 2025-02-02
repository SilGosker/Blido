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
        MethodInfo methodInfo = typeof(ThrowHelper).GetMethod(nameof(ThrowHelper.ThrowUnsupportedException))!;

        // Act
        Action act = () => ThrowHelper.ThrowUnsupportedException(methodInfo);

        // Assert
        var exception = Assert.Throws<NotSupportedException>(act);
        Assert.Equal("Using the method BlazorIndexedOrm.Core.Helpers.ThrowHelper.ThrowUnsupportedException(System.Reflection.MethodInfo) is not supported. Use a different method or overload", exception.Message);
    }
}