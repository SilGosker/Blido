using Blido.Core.Transaction;
using Blido.Core.Transaction.JsExpression;
using Blido.Core.Transaction.Mutation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Moq;

namespace Blido.Core.Options;

public class MutationMiddlewareItemTests
{
    [Fact]
    public void Constructor_WhenTypeAndCreateInstanceIsNull_ShouldThrowArgumentException()
    {
        // Arrange
        Type? type = null;
        object[]? arguments = null;
        Func<IServiceProvider, IBlidoMiddleware>? createInstance = null;

        // Act
        Action action = () => new MutationMiddlewareItem(type, arguments, createInstance);
        
        // Assert
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("Type or CreateInstance must be set.", exception.Message);
    }

    [Fact]
    public void Constructor_WhenArgumentsIsNull_ShouldSetArgumentsToEmptyArray()
    {
        // Arrange
        Type type = typeof(MockBlidoMiddleware);
        object[] arguments = null!;

        // Act
        var mutationMiddlewareItem = new MutationMiddlewareItem(type, arguments);

        // Assert
        Assert.NotNull(mutationMiddlewareItem.Arguments);
        Assert.Empty(mutationMiddlewareItem.Arguments);
    }

    [Fact]
    public void Constructor_WithType_ShouldSetTypeAndArgumentsProperty()
    {
        // Arrange
        Type type = typeof(MockBlidoMiddleware);

        // Act
        var mutationMiddlewareItem = new MutationMiddlewareItem(type);

        // Assert
        Assert.Equal(type, mutationMiddlewareItem.Type);
        Assert.NotNull(mutationMiddlewareItem.Arguments);
        Assert.Empty(mutationMiddlewareItem.Arguments);
        Assert.Null(mutationMiddlewareItem.CreateInstance);
    }

    [Fact]
    public void Constructor_WithCreateInstance_ShouldSetCreateInstanceProperty()
    {
        // Arrange
        Func<IServiceProvider, IBlidoMiddleware> createInstance = _ => new MockBlidoMiddleware();

        // Act
        var mutationMiddlewareItem = new MutationMiddlewareItem(createInstance);

        // Assert
        Assert.Null(mutationMiddlewareItem.Type);
        Assert.NotNull(mutationMiddlewareItem.Arguments);
        Assert.Empty(mutationMiddlewareItem.Arguments);
        Assert.Equal(createInstance, mutationMiddlewareItem.CreateInstance);
    }

    [Fact]
    public async Task InvokeAsync_WhenTypeIsNull_ShouldInvokeCreateInstance()
    {
        // Arrange
        var serviceProvider = new Mock<IServiceProvider>();
        var scope = new Mock<IServiceScope>();
        scope.SetupGet(x => x.ServiceProvider).Returns(serviceProvider.Object);
        var config = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var dbContext = new MockIndexedDbDatabase(new ObjectStoreFactory(new Mock<IExpressionBuilder>().Object, new Mock<IJSRuntime>().Object));
        MutationContext context = new(config, scope.Object, dbContext);
        ProcessNextDelegate processNext = () => ValueTask.CompletedTask;
        int invoked = 0;
        var mutationMiddlewareItem = new MutationMiddlewareItem(_ =>
        {
            invoked++;
            return new Mock<IBlidoMiddleware>().Object;
        });

        // Act
        await mutationMiddlewareItem.InvokeAsync(scope.Object, context, processNext, CancellationToken.None);

        // Assert
        Assert.Equal(1, invoked);
    }

    [Fact]
    public async Task InvokeAsync_WhenTypeIsNotNull_ShouldCreateInstanceAndInvokeExecuteAsync()
    {
        // Arrange
        var serviceProvider = new Mock<IServiceProvider>();
        var middleware = new Mock<IBlidoMiddleware>();
        serviceProvider.Setup(x => x.GetService(It.IsAny<Type>())).Returns(middleware.Object);
        var scope = new Mock<IServiceScope>();
        scope.SetupGet(x => x.ServiceProvider).Returns(serviceProvider.Object);
        var config = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var dbContext = new MockIndexedDbDatabase(new ObjectStoreFactory(new Mock<IExpressionBuilder>().Object, new Mock<IJSRuntime>().Object));
        MutationContext context = new(config, scope.Object, dbContext);
        ProcessNextDelegate processNext = () => ValueTask.CompletedTask;
        var mutationMiddlewareItem = new MutationMiddlewareItem(typeof(MockBlidoMiddleware));

        // Act
        await mutationMiddlewareItem.InvokeAsync(scope.Object, context, processNext, CancellationToken.None);
        
        // Assert
        middleware.Verify(x => x.ExecuteAsync(It.IsAny<MutationContext>(), It.IsAny<ProcessNextDelegate>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}