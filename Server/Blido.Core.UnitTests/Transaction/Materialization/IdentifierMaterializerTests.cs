using Blido.Core.Options;
using Blido.Core.Transaction.JsExpression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Moq;

namespace Blido.Core.Transaction.Materialization;

public class IdentifierMaterializerTests
{
    [Fact]
    public async Task ExecuteAsync_WithNullIdentifiers_ThrowsArgumentNullException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        var transactionProvider = new Mock<ITransactionProvider<object>>();
        objectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntime);
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var options = new Mock<IOptions<MutationConfiguration>>().Object;
        var objectStore = new ObjectStore<object>(new MockIndexedDbDatabase(objectStoreFactory.Object), transactionProvider.Object, serviceProvider, options);
        var expressionBuilder = new Mock<IExpressionBuilder>().Object;
        object identifiers = null!;
        string methodName = "test";
        var cancellationToken = CancellationToken.None;
        
        // Act
        async Task Act() => await IdentifierMaterializer.ExecuteAsync<object, object>(jsRuntime, objectStore,
            expressionBuilder, identifiers, methodName, cancellationToken);
        
        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Act);
    }

    [Fact]
    public async Task ExecuteAsync_WithEmptyMethodName_ThrowsArgumentException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        var transactionProvider = new Mock<ITransactionProvider<object>>();
        objectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntime);
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var options = new Mock<IOptions<MutationConfiguration>>().Object;
        var objectStore = new ObjectStore<object>(new MockIndexedDbDatabase(objectStoreFactory.Object), transactionProvider.Object, serviceProvider, options);
        var expressionBuilder = new Mock<IExpressionBuilder>().Object;
        object identifiers = new object();
        string methodName = string.Empty;
        var cancellationToken = CancellationToken.None;

        // Act
        async Task Act() => await IdentifierMaterializer.ExecuteAsync<object, object>(jsRuntime, objectStore,
            expressionBuilder, identifiers, methodName, cancellationToken);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(Act);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidArguments_ReturnsResult()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>();
        jsRuntime.Setup(x => x.InvokeAsync<object>(It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<object[]>())).ReturnsAsync(true);
        jsRuntime.Setup(x => x.InvokeAsync<ulong>(It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<object[]>())).ReturnsAsync(1ul);
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        var transactionProvider = new Mock<ITransactionProvider<object>>();
        objectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntime.Object);
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var options = new Mock<IOptions<MutationConfiguration>>().Object;
        var objectStore = new ObjectStore<object>(new MockIndexedDbDatabase(objectStoreFactory.Object), transactionProvider.Object, serviceProvider, options);
        var expressionBuilder = new Mock<IExpressionBuilder>().Object;
        object identifiers = new object();
        string methodName = "test";
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await IdentifierMaterializer.ExecuteAsync<object, object>(jsRuntime.Object, objectStore, expressionBuilder, identifiers, methodName, cancellationToken);
        
        // Assert
        Assert.NotNull(result);
    }
}