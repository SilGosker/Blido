using Microsoft.JSInterop;
using Moq;

namespace BlazorIndexedOrm.Core.Transaction.Materialization;

public class FirstOrDefaultTransactionMaterializerTests
{
    [Fact]
    public void Constructor_WhenJsRuntimeIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IJSRuntime jsRuntime = null!;
        var conditions = new TransactionConditions<object>();
        var mockJsRuntime = new Mock<IJSRuntime>().Object;
        var database = new Mock<IndexedDbDatabase>(mockJsRuntime).Object;
        var objectStore = "objectStore";
        // Act
        Action action = () => new FirstOrDefaultTransactionMaterializer<object>(jsRuntime, conditions, database, objectStore);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("jsRuntime", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenConditionsIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        TransactionConditions<object> conditions = null!;
        var mockJsRuntime = new Mock<IJSRuntime>().Object;
        var database = new Mock<IndexedDbDatabase>(mockJsRuntime).Object;
        var objectStore = "objectStore";
        // Act
        Action action = () => new FirstOrDefaultTransactionMaterializer<object>(jsRuntime, conditions, database, objectStore);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("conditions", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenDatabaseIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var conditions = new TransactionConditions<object>();
        IndexedDbDatabase database = null!;
        var objectStore = "objectStore";
        
        // Act
        Action action = () => new FirstOrDefaultTransactionMaterializer<object>(jsRuntime, conditions, database, objectStore);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("database", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenObjectStoreIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var conditions = new TransactionConditions<object>();
        var mockJsRuntime = new Mock<IJSRuntime>().Object;
        var database = new Mock<IndexedDbDatabase>(mockJsRuntime).Object;
        string objectStore = null!;

        // Act
        Action action = () => new FirstOrDefaultTransactionMaterializer<object>(jsRuntime, conditions, database, objectStore);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("objectStore", exception.ParamName);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCalled_CallsJsRuntimeInvokeAsync()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>();
        jsRuntime.Setup(e => e.InvokeAsync<object>(JsMethodNameConstants.GetVersion, It.IsAny<CancellationToken>(), It.IsAny<object[]>())).ReturnsAsync(1ul);
        var database = new Mock<IndexedDbDatabase>(jsRuntime.Object).Object;
        var objectStore = "objectStore";
        var result = new object();
        jsRuntime.Setup(x => x.InvokeAsync<object>(JsMethodNameConstants.FirstOrDefault, It.IsAny<CancellationToken>(), It.IsAny<object[]>())).ReturnsAsync(result);

       var materializer = new FirstOrDefaultTransactionMaterializer<object>(jsRuntime.Object, new TransactionConditions<object>(), database, objectStore);

        // Act
        var actual = await materializer.ExecuteAsync();
        
        // Assert
        Assert.Equal(result, actual);
    }
}