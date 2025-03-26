using Blido.Core.Transaction.Mutation;

namespace Blido.Core.Options;

public class MutationConfigurationTests
{
    [Fact]
    public void UseWithTypeAndArguments_WhenTypeIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var configuration = new MutationConfiguration();
        Type type = null!;
        object[] arguments = Array.Empty<object>();

        // Act
        Action action = () => configuration.Use(type, arguments);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("type", exception.ParamName);
    }

    [Fact]
    public void UseWithTypeAndArguments_WhenArgumentsIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var configuration = new MutationConfiguration();
        Type type = typeof(MockBlidoMiddleware);
        object[] arguments = null!;

        // Act
        Action action = () => configuration.Use(type, arguments);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("arguments", exception.ParamName);
    }

    [Fact]
    public void UseWithTypeAndArguments_WithoutArguments_ShouldNotThrowException()
    {
        // Arrange
        var configuration = new MutationConfiguration();
        Type type = typeof(MockBlidoMiddleware);

        // Act
        configuration.Use(type);

        // Assert
        Assert.Single(configuration.MiddlewareTypes);
    }

    [Fact]
    public void UseWithTypeAndArguments_WhenTypeIsNotAssignableFromBlidoMiddleware_ThrowsInvalidOperationException()
    {
        // Arrange
        var configuration = new MutationConfiguration();
        Type type = typeof(object);
        object[] arguments = Array.Empty<object>();

        // Act
        Action action = () => configuration.Use(type, arguments);

        // Assert
        var exception = Assert.Throws<InvalidOperationException>(action);
        Assert.Equal("The type 'System.Object' is not assignable to type 'Blido.Core.Transaction.Mutation.IBlidoMiddleware'", exception.Message);
    }

    [Fact]
    public void UseWithTypeAndArguments_WhenTypeIsAssignableFromBlidoMiddleware_AddsToMiddlewareTypes()
    {
        // Arrange
        var configuration = new MutationConfiguration();
        Type type = typeof(MockBlidoMiddleware);
        object[] arguments = Array.Empty<object>();

        // Act
        configuration.Use(type, arguments);
        
        // Assert
        var mutationMiddlewareItem = Assert.Single(configuration.MiddlewareTypes);
        Assert.Equal(type, mutationMiddlewareItem.Type);
    }

    [Fact]
    public void UseWithGenericTypeAndArguments_WhenArgumentsIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var configuration = new MutationConfiguration();
        object[] arguments = null!;

        // Act
        Action action = () => configuration.Use<MockBlidoMiddleware>(arguments);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("arguments", exception.ParamName);
    }

    [Fact]
    public void UseWithGenericTypeArguments_WhenArgumentsAreNotNull_ShouldAddToMiddlewareTypes()
    {
        // Arrange
        var configuration = new MutationConfiguration();
        object[] arguments = Array.Empty<object>();

        // Act
        configuration.Use<MockBlidoMiddleware>(arguments);

        // Assert
        var mutationMiddlewareItem = Assert.Single(configuration.MiddlewareTypes);
        Assert.Equal(typeof(MockBlidoMiddleware), mutationMiddlewareItem.Type);
    }

    [Fact]
    public void UseWithCreateInstance_WhenCreateInstanceIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var configuration = new MutationConfiguration();
        Func<IServiceProvider, IBlidoMiddleware> createInstance = null!;

        // Act
        Action action = () => configuration.Use(createInstance);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("createInstance", exception.ParamName);
    }

    [Fact]
    public void UseWithCreateInstance_WhenInstanceIsNotNull_ShouldAddToMiddlewareTypes()
    {
        // Arrange
        var configuration = new MutationConfiguration();
        Func<IServiceProvider, IBlidoMiddleware> createInstance = _ => new MockBlidoMiddleware();

        // Act
        configuration.Use(createInstance);
        
        // Assert
        var mutationMiddlewareItem = Assert.Single(configuration.MiddlewareTypes);
        Assert.Same(createInstance, mutationMiddlewareItem.CreateInstance);
    }
}