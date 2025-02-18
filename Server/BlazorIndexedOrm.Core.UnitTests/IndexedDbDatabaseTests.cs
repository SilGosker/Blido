using BlazorIndexedOrm.Core.Transaction;
using BlazorIndexedOrm.Core.Transaction.JsExpression;
using BlazorIndexedOrm.Core.UnitTests.Mock.IndexedDbDatabase;
using Microsoft.JSInterop;
using Moq;

namespace BlazorIndexedOrm.Core;

public class IndexedDbDatabaseTests
{
    [Fact]
    public void Constructor_WhenPassedNull_ThrowsArgumentNullException()
    {
        // Arrange
        IIndexedDbTransactionProviderFactory mockTransactionProviderFactory = null!;

        // Act
        var act = () => new MockIndexedDbDatabase(mockTransactionProviderFactory);

        // Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("transactionProviderFactory", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenPassedJsRuntime_SetsName()
    {
        // Arrange
        var jsRuntimeMock = new Mock<IJSRuntime>();
        var indexedDbTransactionProviderFactory = new Mock<IIndexedDbTransactionProviderFactory>();
        indexedDbTransactionProviderFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntimeMock.Object);
        
        // Act
        var database = new MockIndexedDbDatabase(indexedDbTransactionProviderFactory.Object);
        
        // Assert
        Assert.NotNull(database.Name);
        Assert.Equal(nameof(MockIndexedDbDatabase), database.Name);
    }

    [Fact]
    public void Constructor_WhenChildHasIndexedDbDatabaseNameAttribute_SetsName()
    {
        // Act
        var jsRuntime = new Mock<IJSRuntime>();
        var indexedDbTransactionProviderFactory = new Mock<IIndexedDbTransactionProviderFactory>();
        indexedDbTransactionProviderFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntime.Object);
        
        // Act
        var database = new MockIndexedDbDatabaseWithAttribute(indexedDbTransactionProviderFactory.Object);

        // Assert
        Assert.NotNull(database.Name);
        Assert.Equal("CustomName", database.Name);
    }

    [Fact]
    public void Constructor_SetsVersion()
    {
        // Arrange
        ulong version = 1;
        var jsRuntimeMock = new Mock<IJSRuntime>();
        jsRuntimeMock.Setup(x => x.InvokeAsync<ulong>(JsMethodNameConstants.GetVersion, It.IsAny<object[]>()))
            .ReturnsAsync(version);
        var indexedDbTransactionProviderFactoryMock = new Mock<IIndexedDbTransactionProviderFactory>();
        indexedDbTransactionProviderFactoryMock.Setup(x => x.JsRuntime).Returns(jsRuntimeMock.Object);

        // Act
        var database = new MockIndexedDbDatabase(indexedDbTransactionProviderFactoryMock.Object);

        // Assert
        Assert.Equal(version, database.Version);
    }

    [Fact]
    public void Constructor_SetsObjectStores()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>();
        var jsExpressionBuilder = new Mock<IExpressionBuilder>();
        var indexedDbTransactionProviderFactoryMock = new Mock<IIndexedDbTransactionProviderFactory>();
        indexedDbTransactionProviderFactoryMock.SetupGet(e => e.JsRuntime).Returns(jsRuntime.Object);
        MockIndexedDbDatabase mockDatabase = new(indexedDbTransactionProviderFactoryMock.Object);
        indexedDbTransactionProviderFactoryMock.Setup(e => e.GetIndexedDbTransactionProvider(It.IsAny<Type>())).Returns(
            (Type type) =>
            {
                if (type == typeof(Exception))
                {
                    return new IndexedDbTransactionProvider<Exception>(jsRuntime.Object, mockDatabase, jsExpressionBuilder.Object);
                }
                return new IndexedDbTransactionProvider<string>(jsRuntime.Object, mockDatabase, jsExpressionBuilder.Object);
            });

        // Act
        var database = new MockIndexedDbDatabaseWithObjectStoreSetProperties(indexedDbTransactionProviderFactoryMock.Object);

        // Assert
        Assert.NotNull(database.Exceptions);
        Assert.NotNull(database.Strings);
    }

    [Fact]
    public async Task GetVersionAsync_ReturnsVersion()
    {
        // Arrange
        ulong version = 1;
        var jsRuntimeMock = new Mock<IJSRuntime>();
        jsRuntimeMock.Setup(x => x.InvokeAsync<ulong>(JsMethodNameConstants.GetVersion, It.IsAny<object[]>()))
            .ReturnsAsync(version);
        var indexedDbTransactionProviderFactoryMock = new Mock<IIndexedDbTransactionProviderFactory>();
        indexedDbTransactionProviderFactoryMock.Setup(x => x.JsRuntime).Returns(jsRuntimeMock.Object);
        var database = new MockIndexedDbDatabase(indexedDbTransactionProviderFactoryMock.Object);

        // Act
        var result = await database.GetVersionAsync();
        
        // Assert
        Assert.Equal(version, result);
    }
}