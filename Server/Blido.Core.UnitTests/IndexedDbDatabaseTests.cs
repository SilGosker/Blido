using Blido.Core.Transaction;
using Microsoft.JSInterop;
using Moq;

namespace Blido.Core;

public class IndexedDbDatabaseTests
{
    [Fact]
    public void Constructor_WithNullJsRuntime_ThrowsArgumentNullException()
    {
        // Arrange
        IJSRuntime mockJsRuntime = null!;
        var factory = new Mock<IObjectStoreFactory>();
        factory.SetupGet(x => x.JsRuntime).Returns(new Mock<IJSRuntime>().Object);
        IndexedDbContext mockContext = new MockIndexedDbDatabase(factory.Object);

        // Act
        var act = () => new IndexedDbDatabase(mockContext, mockJsRuntime);
        
        // Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("jsRuntime", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullContext_ThrowsArgumentNullException()
    {
        // Arrange
        IndexedDbContext mockContext = null!;
        var mockJsRuntime = new Mock<IJSRuntime>();

        // Act
        var act = () => new IndexedDbDatabase(mockContext, mockJsRuntime.Object);

        // Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("dbContext", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithDatabase_SetsName()
    {
        // Arrange
        var mockJsRuntime = new Mock<IJSRuntime>();
        var objectStoreFactoryMock = new Mock<IObjectStoreFactory>();
        objectStoreFactoryMock.SetupGet(x => x.JsRuntime).Returns(mockJsRuntime.Object);
        var indexedDbContext = new MockIndexedDbDatabase(objectStoreFactoryMock.Object);

        // Act
        var database = new IndexedDbDatabase(indexedDbContext, mockJsRuntime.Object);
        
        // Assert
        Assert.Equal("MockIndexedDbDatabase", database.Name);
    }

    [Fact]
    public void Constructor_WhenDatabaseHasIndexedDbDatabaseNameAttribute_SetsName()
    {
        // Arrange
        var mockJsRuntime = new Mock<IJSRuntime>();
        var objectStoreFactoryMock = new Mock<IObjectStoreFactory>();
        objectStoreFactoryMock.SetupGet(x => x.JsRuntime).Returns(mockJsRuntime.Object);
        var indexedDbContext = new MockIndexedDbDatabaseWithAttribute(objectStoreFactoryMock.Object);

        // Act
        var database = new IndexedDbDatabase(indexedDbContext, mockJsRuntime.Object);

        // Assert
        Assert.Equal("CustomName", database.Name);
    }

    [Fact]
    public async Task GetVersionAsync_InvokesJsRuntime()
    {
        // Arrange
        var mockJsRuntime = new Mock<IJSRuntime>();
        mockJsRuntime.Setup(x => x.InvokeAsync<ulong>(It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<object[]>())).ReturnsAsync(1ul);
        var objectStoreFactoryMock = new Mock<IObjectStoreFactory>();
        objectStoreFactoryMock.SetupGet(x => x.JsRuntime).Returns(mockJsRuntime.Object);
        var indexedDbContext = new MockIndexedDbDatabase(objectStoreFactoryMock.Object);
        var database = new IndexedDbDatabase(indexedDbContext, mockJsRuntime.Object);

        // Act
        var version = await database.GetVersionAsync();
        
        // Assert
        mockJsRuntime.Verify(x => x.InvokeAsync<ulong>(It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<object[]>()), Times.Once);
        Assert.Equal(1ul, version);
    }
}