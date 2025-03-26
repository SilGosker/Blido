using Blido.Core.Options;
using Blido.Core.Transaction.Configuration;
using Blido.Core.Transaction.JsExpression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System.Text.Json;
using Moq;

namespace Blido.Core.Transaction.Materialization;

public class MaterializerTests
{
    [Fact]
    public async Task ExecuteAsync_WithValidArguments_ShouldReturnResult()
    {
        // Arrange
        var jsRuntimeMock = new Mock<IJSRuntime>();
        var expressionBuilder = new Mock<IExpressionBuilder>();
        var mockObjectStoreFactory = new Mock<IObjectStoreFactory>();
        mockObjectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntimeMock.Object);
        var context = new MockIndexedDbDatabase(mockObjectStoreFactory.Object);
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var objectStore = new ObjectStore<object>(context, new QueryProvider<object>(jsRuntimeMock.Object, expressionBuilder.Object), serviceProvider, options);
        var conditions = new TransactionConditions<object>();
        var methodName = "methodName";
        var cancellationToken = CancellationToken.None;
        jsRuntimeMock.Setup(x => x.InvokeAsync<object>(methodName, cancellationToken, It.IsAny<object[]>()))
            .ReturnsAsync(new object());
        // Act
        var result = await Materializer.ExecuteAsync<object, object>(jsRuntimeMock.Object, objectStore, expressionBuilder.Object, conditions, methodName, cancellationToken);
        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task ExecuteAsync_WithNullConditions_ShouldThrowArgumentNullException()
    {
        // Arrange
        var jsRuntimeMock = new Mock<IJSRuntime>();
        var expressionBuilder = new Mock<IExpressionBuilder>();
        var mockObjectStoreFactory = new Mock<IObjectStoreFactory>();
        mockObjectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntimeMock.Object);
        var context = new MockIndexedDbDatabase(mockObjectStoreFactory.Object);
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var objectStore = new ObjectStore<object>(context, new QueryProvider<object>(jsRuntimeMock.Object, expressionBuilder.Object), serviceProvider, options);
        TransactionConditions<object> conditions = null!;
        var methodName = "methodName";
        var cancellationToken = CancellationToken.None;
        
        // Act
        async Task Act() => await Materializer.ExecuteAsync<object, object>(jsRuntimeMock.Object, objectStore, expressionBuilder.Object, conditions, methodName, cancellationToken);
        
        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Act);
    }

    [Fact]
    public async Task ExecuteAsync_WithNullMethodName_ShouldThrowArgumentNullException()
    {
        // Arrange
        var jsRuntimeMock = new Mock<IJSRuntime>();
        var expressionBuilder = new Mock<IExpressionBuilder>();
        var mockObjectStoreFactory = new Mock<IObjectStoreFactory>();
        mockObjectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntimeMock.Object);
        var context = new MockIndexedDbDatabase(mockObjectStoreFactory.Object);
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var objectStore = new ObjectStore<object>(context, new QueryProvider<object>(jsRuntimeMock.Object, expressionBuilder.Object), serviceProvider, options);
        var conditions = new TransactionConditions<object>();
        string methodName = null!;
        var cancellationToken = CancellationToken.None;
        
        // Act
        async Task Act() => await Materializer.ExecuteAsync<object, object>(jsRuntimeMock.Object, objectStore, expressionBuilder.Object, conditions, methodName, cancellationToken);
        
        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Act);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public async Task ExecuteAsync_WithEmptyMethodName_ShouldThrowArgumentException(string methodName)
    {
        // Arrange
        var jsRuntimeMock = new Mock<IJSRuntime>();
        var expressionBuilder = new Mock<IExpressionBuilder>();
        var mockObjectStoreFactory = new Mock<IObjectStoreFactory>();
        mockObjectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntimeMock.Object);
        var context = new MockIndexedDbDatabase(mockObjectStoreFactory.Object);
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var objectStore = new ObjectStore<object>(context, new QueryProvider<object>(jsRuntimeMock.Object, expressionBuilder.Object), serviceProvider, options);
        var conditions = new TransactionConditions<object>();
        var cancellationToken = CancellationToken.None;

        // Act
        async Task Act() => await Materializer.ExecuteAsync<object, object>(jsRuntimeMock.Object, objectStore, expressionBuilder.Object, conditions, methodName, cancellationToken);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(Act);
    }

    [Fact]
    public async Task ExecuteAsync_WithNoConditions_ShouldLeaveParsedExpressionsNull()
    {
        // Arrange
        var jsRuntimeMock = new Mock<IJSRuntime>();
        var expressionBuilder = new Mock<IExpressionBuilder>();
        var mockObjectStoreFactory = new Mock<IObjectStoreFactory>();
        mockObjectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntimeMock.Object);
        var context = new MockIndexedDbDatabase(mockObjectStoreFactory.Object);
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var objectStore = new ObjectStore<object>(context, new QueryProvider<object>(jsRuntimeMock.Object, expressionBuilder.Object), serviceProvider, options);
        var conditions = new TransactionConditions<object>();
        var methodName = "methodName";
        var cancellationToken = CancellationToken.None;
        MaterializationArguments? arguments = null;

        jsRuntimeMock.Setup(x => x.InvokeAsync<object>(methodName, cancellationToken, It.IsAny<object[]>()))
            .ReturnsAsync((string _, CancellationToken _, object[] args) =>
            {
                arguments = JsonSerializer.Deserialize<MaterializationArguments>((string)args[0]);
                return new object();
            });

        // Act
        await Materializer.ExecuteAsync<object, object>(jsRuntimeMock.Object, objectStore, expressionBuilder.Object, conditions, methodName, cancellationToken);
        
        // Assert
        Assert.NotNull(arguments);
        Assert.Null(arguments!.ParsedExpressions);
    }

    [Fact]
    public async Task ExecuteAsync_WithConditions_ParsesAndPassesConditions()
    {
        // Arrange
        var jsRuntimeMock = new Mock<IJSRuntime>();
        var expressionBuilder = new Mock<IExpressionBuilder>();
        var mockObjectStoreFactory = new Mock<IObjectStoreFactory>();
        mockObjectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntimeMock.Object);
        var context = new MockIndexedDbDatabase(mockObjectStoreFactory.Object);
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var objectStore = new ObjectStore<object>(context, new QueryProvider<object>(jsRuntimeMock.Object, expressionBuilder.Object), serviceProvider, options);
        var conditions = new TransactionConditions<object>();
        MaterializationArguments? arguments = null;

        conditions.AddCondition(x => x != null!);
        conditions.AddCondition(x => x == null!);
        
        jsRuntimeMock.Setup(x => x.InvokeAsync<object>(It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<object[]>()))
            .ReturnsAsync((string _, CancellationToken _, object[] args) =>
            {
                arguments = JsonSerializer.Deserialize<MaterializationArguments>((string)args[0]);
                return new object();
            });

        jsRuntimeMock.Setup(x => x.InvokeAsync<ulong>(It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<object[]>()))
            .ReturnsAsync((string _, CancellationToken _, object[] _) => 1ul);

        // Act
        await Materializer.ExecuteAsync<object, object>(jsRuntimeMock.Object, objectStore, expressionBuilder.Object, conditions, "methodName", CancellationToken.None);

        // Assert
        Assert.NotNull(arguments);
        Assert.NotNull(arguments!.ParsedExpressions);
        Assert.Equal(2, arguments!.ParsedExpressions!.Length);
    }
}