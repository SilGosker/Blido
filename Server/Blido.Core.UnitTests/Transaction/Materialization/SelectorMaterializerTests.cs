using System.Linq.Expressions;
using System.Text.Json;
using Blido.Core.Transaction.Configuration;
using Blido.Core.Transaction.JsExpression;
using Microsoft.JSInterop;
using Moq;

namespace Blido.Core.Transaction.Materialization;

public class SelectorMaterializerTests
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
        var objectStore = new ObjectStore<object>(database, new TransactionProvider<object>(jsRuntimeMock.Object, database, expressionBuilder.Object));
        var conditions = new TransactionConditions<object>();
        var methodName = "methodName";
        var cancellationToken = CancellationToken.None;
        jsRuntimeMock.Setup(x => x.InvokeAsync<object>(methodName, cancellationToken, It.IsAny<object[]>()))
            .ReturnsAsync(new object());

        // Act
        var result = await SelectorMaterializer.ExecuteAsync<object, object>(jsRuntimeMock.Object,
            objectStore,
            expressionBuilder.Object,
            conditions,
            x => x,
            methodName,
            cancellationToken);
        
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
        var objectStore = new ObjectStore<object>(database, new TransactionProvider<object>(jsRuntimeMock.Object, database, expressionBuilder.Object));
        TransactionConditions<object> conditions = null!;
        var methodName = "methodName";
        var cancellationToken = CancellationToken.None;

        // Act
        async Task Act() => await SelectorMaterializer.ExecuteAsync<object, object>(jsRuntimeMock.Object,
            objectStore,
            expressionBuilder.Object,
            conditions,
            x => x,
            methodName,
            cancellationToken);

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
        var objectStore = new ObjectStore<object>(database, new TransactionProvider<object>(jsRuntimeMock.Object, database, expressionBuilder.Object));
        var conditions = new TransactionConditions<object>();
        string methodName = null!;
        var cancellationToken = CancellationToken.None;
        
        // Act
        async Task Act() => await SelectorMaterializer.ExecuteAsync<object, object>(jsRuntimeMock.Object,
            objectStore,
            expressionBuilder.Object,
            conditions,
            x => x,
            methodName,
            cancellationToken);
        
        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Act);
    }

    [Fact]
    public async Task ExecuteAsync_WithNullSelector_ShouldThrowArgumentNullException()
    {
        // Arrange
        var jsRuntimeMock = new Mock<IJSRuntime>();
        var expressionBuilder = new Mock<IExpressionBuilder>();
        var database =
            new IndexedDbDatabase(
                new MockIndexedDbDatabase(new ObjectStoreFactory(expressionBuilder.Object, jsRuntimeMock.Object)),
                jsRuntimeMock.Object);
        var objectStore = new ObjectStore<object>(database, new TransactionProvider<object>(jsRuntimeMock.Object, database, expressionBuilder.Object));
        var conditions = new TransactionConditions<object>();
        var methodName = "methodName";
        var cancellationToken = CancellationToken.None;
        
        // Act
        async Task Act() => await SelectorMaterializer.ExecuteAsync<object, object>(jsRuntimeMock.Object,
            objectStore,
            expressionBuilder.Object,
            conditions,
            null!,
            methodName,
            cancellationToken);
        
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
        var objectStore = new ObjectStore<object>(database, new TransactionProvider<object>(jsRuntimeMock.Object, database, expressionBuilder.Object));
        var conditions = new TransactionConditions<object>();
        var cancellationToken = CancellationToken.None; 
        
        // Act
        async Task Act() => await SelectorMaterializer.ExecuteAsync<object, object>(jsRuntimeMock.Object,
            objectStore,
            expressionBuilder.Object,
            conditions,
            x => x,
            methodName,
            cancellationToken);
       
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
        var objectStore = new ObjectStore<object>(database, new TransactionProvider<object>(jsRuntimeMock.Object, database, expressionBuilder.Object));
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
        await SelectorMaterializer.ExecuteAsync<object, object>(jsRuntimeMock.Object,
            objectStore,
            expressionBuilder.Object,
            conditions,
            x => x,
            methodName,
            cancellationToken);

        // Assert
        Assert.NotNull(arguments);
        Assert.Null(arguments!.ParsedExpressions);
    }

    [Fact]
    public async Task ExecuteAsync_WithConditions_ShouldSetParsedExpressions()
    {
        // Arrange
        var jsRuntimeMock = new Mock<IJSRuntime>();
        var expressionBuilder = new Mock<IExpressionBuilder>();
        var database =
            new IndexedDbDatabase(
                new MockIndexedDbDatabase(new ObjectStoreFactory(expressionBuilder.Object, jsRuntimeMock.Object)),
                jsRuntimeMock.Object);
        var objectStore = new ObjectStore<object>(database, new TransactionProvider<object>(jsRuntimeMock.Object, database, expressionBuilder.Object));
        var conditions = new TransactionConditions<object>();
        conditions.AddCondition(x => true);
        conditions.AddCondition(x => false);
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
        await SelectorMaterializer.ExecuteAsync<object, object>(jsRuntimeMock.Object,
            objectStore,
            expressionBuilder.Object,
            conditions,
            x => x,
            methodName,
            cancellationToken);

        // Assert
        Assert.NotNull(arguments);
        Assert.NotNull(arguments!.ParsedExpressions);
        Assert.Equal(2, arguments!.ParsedExpressions!.Length);
    }

    [Fact]
    public async Task ExecuteAsync_WithSelector_ShouldSetSelector()
    {
        // Arrange
        var jsRuntimeMock = new Mock<IJSRuntime>();
        var expressionBuilder = new Mock<IExpressionBuilder>();
        expressionBuilder.Setup(x => x.ProcessExpression(It.IsAny<LambdaExpression>())).Returns("x=>x");
        var database =
            new IndexedDbDatabase(
                new MockIndexedDbDatabase(new ObjectStoreFactory(expressionBuilder.Object, jsRuntimeMock.Object)),
                jsRuntimeMock.Object);
        var objectStore = new ObjectStore<object>(database, new TransactionProvider<object>(jsRuntimeMock.Object, database, expressionBuilder.Object));
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
        await SelectorMaterializer.ExecuteAsync<object, object>(jsRuntimeMock.Object,
            objectStore,
            expressionBuilder.Object,
            conditions,
            x => x,
            methodName,
            cancellationToken);

        // Assert
        Assert.NotNull(arguments);
        Assert.NotNull(arguments!.Selector);

    }
}