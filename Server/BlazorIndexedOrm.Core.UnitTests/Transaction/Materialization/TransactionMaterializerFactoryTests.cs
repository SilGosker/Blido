using BlazorIndexedOrm.Core.Transaction;
using BlazorIndexedOrm.Core.Transaction.Materialization;
using Microsoft.JSInterop;
using Moq;

namespace BlazorIndexedOrm.Core.UnitTests.Transaction.Materialization;

public class TransactionMaterializerFactoryTests
{
    [Fact]
    public void Constructor_WhenJsRuntimeIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IJSRuntime jsRuntime = null!;
        var conditions = new TransactionConditions<object>();
        var mockJsRuntime = new Mock<IJSRuntime>().Object;
        var database = new Mock<IndexedDbDatabase>(mockJsRuntime).Object;
        // Act
        Action action = () => new TransactionMaterializerFactory<object>(jsRuntime, conditions, database);

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
        // Act
        Action action = () => new TransactionMaterializerFactory<object>(jsRuntime, conditions, database);
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
        // Act
        Action action = () => new TransactionMaterializerFactory<object>(jsRuntime, conditions, database);
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("database", exception.ParamName);
    }

    [Fact]
    public void GetAggregator_WhenMethodNameIsFirstOrDefaultAsync_ReturnsFirstOrDefaultTransactionMaterializer()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var conditions = new TransactionConditions<object>();
        var mockJsRuntime = new Mock<IJSRuntime>().Object;
        var database = new Mock<IndexedDbDatabase>(mockJsRuntime).Object;
        var factory = new TransactionMaterializerFactory<object>(jsRuntime, conditions, database);
        var methodName = nameof(ITransactionMaterializationProvider<object>.FirstOrDefaultAsync);
        // Act
        var result = factory.GetMaterializer<object>(methodName);
        // Assert
        Assert.IsType<FirstOrDefaultTransactionMaterializer<object>>(result);
    }
}