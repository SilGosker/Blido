using BlazorIndexedOrm.Core.Transaction;
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
        IJSRuntime jsRuntime = null!;

        // Act
        var act = () => new MockIndexedDbDatabase(jsRuntime);

        // Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("jsRuntime", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenPassedJsRuntime_SetsName()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;

        // Act
        var database = new MockIndexedDbDatabase(jsRuntime);
        
        // Assert
        Assert.NotNull(database.Name);
        Assert.Equal(nameof(MockIndexedDbDatabase), database.Name);
    }

    [Fact]
    public void Constructor_WhenChildHasIndexedDbDatabaseNameAttribute_SetsName()
    {
        // Act
        var jsRuntime = new Mock<IJSRuntime>().Object;

        // Act
        var database = new MockIndexedDbDatabaseWithAttribute(jsRuntime);

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
        var jsRuntime = jsRuntimeMock.Object;

        // Act
        var database = new MockIndexedDbDatabase(jsRuntime);

        // Assert
        Assert.Equal(version, database.Version);
    }

    [Fact]
    public void Constructor_SetsObjectStores()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;

        // Act
        var database = new MockIndexedDbDatabaseWithObjectStoreSetProperties(jsRuntime);

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
        var jsRuntime = jsRuntimeMock.Object;
        var database = new MockIndexedDbDatabase(jsRuntime);

        // Act
        var result = await database.GetVersionAsync();
        
        // Assert
        Assert.Equal(version, result);
    }
}