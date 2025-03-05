using System.Linq.Expressions;
using Blido.Core.Transaction.JsExpression;
using Microsoft.JSInterop;
using Moq;

namespace Blido.Core.Transaction;

public class TransactionProviderTests
{
    [Fact]
    public void Constructor_WithNullJsRuntime_ShouldThrowArgumentNullException()
    {
        // Arrange
        IJSRuntime jsRuntime = null!;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.Setup(x => x.JsRuntime).Returns(new Mock<IJSRuntime>().Object);
        var database = new IndexedDbDatabase(new MockIndexedDbDatabase(objectStoreFactory.Object), new Mock<IJSRuntime>().Object);
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        
        // Act
        Action act = () => new TransactionProvider<object>(jsRuntime, database, jsExpressionBuilder);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("jsRuntime", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullDatabase_ShouldThrowArgumentNullException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.Setup(x => x.JsRuntime).Returns(jsRuntime);
        IndexedDbDatabase database = null!;
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;

        // Act
        Action act = () => new TransactionProvider<object>(jsRuntime, database, jsExpressionBuilder);
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("database", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullJsExpressionBuilder_ShouldThrowArgumentNullException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.Setup(x => x.JsRuntime).Returns(jsRuntime);
        var database = new IndexedDbDatabase(new MockIndexedDbDatabase(objectStoreFactory.Object), jsRuntime);
        IExpressionBuilder jsExpressionBuilder = null!;

        // Act
        Action act = () => new TransactionProvider<object>(jsRuntime, database, jsExpressionBuilder);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("jsExpressionBuilder", exception.ParamName);
    }

    [Fact]
    public void Where_WithNullExpression_ShouldThrowArgumentNullException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.Setup(x => x.JsRuntime).Returns(jsRuntime);
        var database = new IndexedDbDatabase(new MockIndexedDbDatabase(objectStoreFactory.Object), jsRuntime);
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        var transactionProvider = new TransactionProvider<object>(jsRuntime, database, jsExpressionBuilder);

        // Act
        Action act = () => transactionProvider.Where(null!);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("expression", exception.ParamName);
    }

    [Fact]
    public async Task ExecuteAsync_WithNullMethodName_ShouldThrowArgumentException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.Setup(x => x.JsRuntime).Returns(jsRuntime);
        var database = new IndexedDbDatabase(new MockIndexedDbDatabase(objectStoreFactory.Object), jsRuntime); var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        var transactionProvider = new TransactionProvider<object>(jsRuntime, database, jsExpressionBuilder);
        // Act
        Func<Task> act = async () => await transactionProvider.ExecuteAsync<object>(null!, CancellationToken.None);
        // Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(act);
        Assert.Equal("methodName", exception.ParamName);
    }

    [Fact]
    public async Task ExecuteAsync_WithNullSelector_ShouldThrowArgumentNullException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.Setup(x => x.JsRuntime).Returns(jsRuntime);
        var database = new IndexedDbDatabase(new MockIndexedDbDatabase(objectStoreFactory.Object), jsRuntime); var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        var transactionProvider = new TransactionProvider<object>(jsRuntime, database, jsExpressionBuilder);
        // Act
        Func<Task> act = async () => await transactionProvider.ExecuteAsync<object>("methodName", null!, CancellationToken.None);
        // Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(act);
        Assert.Equal("selector", exception.ParamName);
    }

    [Fact]
    public async Task ExecuteAsync_WithUnsupportedMethodName_ShouldThrowArgumentException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.Setup(x => x.JsRuntime).Returns(jsRuntime);
        var database = new IndexedDbDatabase(new MockIndexedDbDatabase(objectStoreFactory.Object), jsRuntime); var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        var transactionProvider = new TransactionProvider<object>(jsRuntime, database, jsExpressionBuilder);
        // Act
        Func<Task> act = async () => await transactionProvider.ExecuteAsync<object>("unsupportedMethodName", CancellationToken.None);
        // Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(act);
        Assert.Equal("Method name 'unsupportedMethodName' is not supported.", exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidArguments_ShouldNotThrow()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>();
        jsRuntime.Setup(x => x.InvokeAsync<bool>(It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<object[]>())).ReturnsAsync(true);
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.Setup(x => x.JsRuntime).Returns(jsRuntime.Object);
        var database = new IndexedDbDatabase(new MockIndexedDbDatabase(objectStoreFactory.Object), jsRuntime.Object); var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        var transactionProvider = new TransactionProvider<object>(jsRuntime.Object, database, jsExpressionBuilder);
        ((ITransactionProvider)transactionProvider).SetObjectStore(new ObjectStore<object>(database, transactionProvider));
        // Act
        var result = await transactionProvider.ExecuteAsync<bool>("AnyAsync");
        
        // Assert
        Assert.True(result);
    }
}