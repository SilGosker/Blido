using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Blido.Core.Transaction.Configuration;
using Blido.Core.Transaction.JsExpression;
using Microsoft.JSInterop;
using Moq;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Blido.Core.Transaction.Materialization;

public class MaterializerTests
{
    [Fact]
    public async Task ExecuteAsync_WithValidArguments_ShouldReturnResult()
    {
        // Arrange
        var jsRuntimeMock = new Mock<IJSRuntime>();
        var expressionBuilder = new Mock<IExpressionBuilder>();
        var database =
            new IndexedDbDatabase(
                new MockIndexedDbDatabase(new ObjectStoreFactory(expressionBuilder.Object, jsRuntimeMock.Object)),
                jsRuntimeMock.Object);
        var objectStore = new ObjectStore<object>(database, new QueryProvider<object>(jsRuntimeMock.Object, expressionBuilder.Object));
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
        var database =
            new IndexedDbDatabase(
                new MockIndexedDbDatabase(new ObjectStoreFactory(expressionBuilder.Object, jsRuntimeMock.Object)),
                jsRuntimeMock.Object);
        var objectStore = new ObjectStore<object>(database, new QueryProvider<object>(jsRuntimeMock.Object, expressionBuilder.Object));
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
        var database =
            new IndexedDbDatabase(
                new MockIndexedDbDatabase(new ObjectStoreFactory(expressionBuilder.Object, jsRuntimeMock.Object)),
                jsRuntimeMock.Object);
        var objectStore = new ObjectStore<object>(database, new QueryProvider<object>(jsRuntimeMock.Object, expressionBuilder.Object));
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
        var database =
            new IndexedDbDatabase(
                new MockIndexedDbDatabase(new ObjectStoreFactory(expressionBuilder.Object, jsRuntimeMock.Object)),
                jsRuntimeMock.Object);
        var objectStore = new ObjectStore<object>(database, new QueryProvider<object>(jsRuntimeMock.Object, expressionBuilder.Object));
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
        var database =
            new IndexedDbDatabase(
                new MockIndexedDbDatabase(new ObjectStoreFactory(expressionBuilder.Object, jsRuntimeMock.Object)),
                jsRuntimeMock.Object);
        var objectStore = new ObjectStore<object>(database, new QueryProvider<object>(jsRuntimeMock.Object, expressionBuilder.Object));
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
        var database =
            new IndexedDbDatabase(
                new MockIndexedDbDatabase(new ObjectStoreFactory(expressionBuilder.Object, jsRuntimeMock.Object)),
                jsRuntimeMock.Object);
        var objectStore = new ObjectStore<object>(database, new QueryProvider<object>(jsRuntimeMock.Object, expressionBuilder.Object));
        var conditions = new TransactionConditions<object>();
        var methodName = "methodName";
        var cancellationToken = CancellationToken.None;
        MaterializationArguments? arguments = null;

        conditions.AddCondition(x => x != null!);
        conditions.AddCondition(x => x == null!);
        
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
        Assert.NotNull(arguments!.ParsedExpressions);
        Assert.Equal(2, arguments!.ParsedExpressions!.Length);
    }
}