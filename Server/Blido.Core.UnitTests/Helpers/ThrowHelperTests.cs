using System.Collections.Frozen;
using System.Linq.Expressions;
using System.Reflection;
using Blido.Core.Transaction;
using Blido.Core.Transaction.JsExpression;
using Blido.Core.Transaction.Mutation.KeyGeneration.KeyGenerators;
using Microsoft.JSInterop;
using Moq;

namespace Blido.Core.Helpers;

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
        Assert.Equal("Using the method Blido.Core.Helpers.ThrowHelper.ThrowUnsupportedException(System.Reflection.MethodInfo) is not supported. Use a different method or overload", exception.Message);
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
    public void ThrowUnSupportedException_WithNullUnaryExpression_ShouldThrowArgumentNullException()
    {
        // Arrange
        UnaryExpression unary = null!;

        // Act
        Action act = () => ThrowHelper.ThrowUnsupportedException(unary);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("unary", exception.ParamName);
    }

    [Fact]
    public void ThrowUnSupportedException_WithUnaryExpression_ShouldThrowUnSupportedException()
    {
        // Arrange
        var unary = Expression.Not(Expression.Constant(true));
        
        // Act
        Action act = () => ThrowHelper.ThrowUnsupportedException(unary);
        
        // Assert
        var exception = Assert.Throws<NotSupportedException>(act);
        Assert.Equal("Using the unary expression Not(System.Boolean) is not supported. Use a different expression or overload", exception.Message);
    }

    [Fact]
    public void ThrowUnSupportedException_WithConvertUnaryExpression_ShouldThrowUnSupportedException()
    {
        // Arrange
        var unary = Expression.Convert(Expression.Constant(1), typeof(long));

        // Act
        Action act = () => ThrowHelper.ThrowUnsupportedException(unary);

        // Assert
        var exception = Assert.Throws<NotSupportedException>(act);
        Assert.Equal("Using the unary expression Convert(System.Int32 to System.Int64) is not supported. Use a different expression or overload", exception.Message);
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

    [Fact]
    public void ThrowTypeNotInObjectStores_WhenTypeIsNotInObjectStoreProperties_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var entityType = typeof(ThrowHelper);
        var context = new MockIndexedDbDatabase(new ObjectStoreFactory(new Mock<IExpressionBuilder>().Object, new Mock<IJSRuntime>().Object));

        // Act
        Action act = () => ThrowHelper.ThrowTypeNotInObjectStores(entityType, context);
        
        // Assert
        var exception = Assert.Throws<InvalidOperationException>(act);
        Assert.Equal($"The entity of type Blido.Core.Helpers.ThrowHelper is not present in any of the objectstores in context Blido.Core.MockIndexedDbDatabase", exception.Message);
    }

    [Fact]
    public void ThrowTypeNotInObjectStores_WhenTypeIsInObjectStoreProperties_ShouldNotThrowException()
    {
        // Arrange
        var entityType = typeof(EntityWithGuidKey);
        var context = new MockIndexedDbDatabaseWithPrimaryKeySetProperties(new ObjectStoreFactory(new Mock<IExpressionBuilder>().Object, new Mock<IJSRuntime>().Object));
        
        // Act
        Action act = () => ThrowHelper.ThrowTypeNotInObjectStores(entityType, context);
        
        // Assert
        act();
    }

    [Fact]
    public void ThrowIfNotAssignableTo_WhenTypeIsAssignableFromGeneric_DoesNotThrow()
    {
        // Arrange
        var type = typeof(List<int>);

        // Act
        ThrowHelper.ThrowIfNotAssignableTo<ICollection<int>>(type);
    }

    [Fact]
    public void ThrowIfNotAssignableTo_WhenTypeIsNotAssignableFromGeneric_ShouldThrowNotSupportedException()
    {
        // Arrange
        var type = typeof(EntityWithGuidKey);

        // Act
        Action act = () => ThrowHelper.ThrowIfNotAssignableTo<EntityWithNumberKey>(type);

        // Assert
        var exception = Assert.Throws<InvalidOperationException>(act);
        Assert.Equal("The type 'Blido.Core.Transaction.Mutation.KeyGeneration.KeyGenerators.EntityWithGuidKey' is not assignable to type 'Blido.Core.Transaction.Mutation.KeyGeneration.KeyGenerators.EntityWithNumberKey'", exception.Message);
    }
}