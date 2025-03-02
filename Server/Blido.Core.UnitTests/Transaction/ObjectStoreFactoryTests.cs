using Blido.Core.Transaction.JsExpression;
using Microsoft.JSInterop;
using Moq;

namespace Blido.Core.Transaction;

public class ObjectStoreFactoryTests
{
    [Fact]
    public void Constructor_WhenExpressionBuilderIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IExpressionBuilder expressionBuilder = null!;
        var jsRuntime = new Mock<IJSRuntime>().Object;
     
        // Act
        var action = () => new ObjectStoreFactory(expressionBuilder, jsRuntime);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("expressionBuilder", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenJsRuntimeIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var expressionBuilder = new Mock<IExpressionBuilder>().Object;
        IJSRuntime jsRuntime = null!;

        // Act
        var action = () => new ObjectStoreFactory(expressionBuilder, jsRuntime);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("jsRuntime", exception.ParamName);
    }

    [Fact]
    public void GetObjectStore_WhenDatabaseIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var expressionBuilder = new Mock<IExpressionBuilder>().Object;
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var factory = new ObjectStoreFactory(expressionBuilder, jsRuntime);
        IndexedDbDatabase database = null!;
        var entityType = typeof(object);

        // Act
        var action = () => ((IObjectStoreFactory)factory).GetObjectStore(database, entityType);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("database", exception.ParamName);
    }

    [Fact]
    public void GetObjectStore_WhenEntityTypeIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var expressionBuilder = new Mock<IExpressionBuilder>().Object;
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var factory = new ObjectStoreFactory(expressionBuilder, jsRuntime);
        var database = new IndexedDbDatabase(new MockIndexedDbDatabase(factory), jsRuntime);
        Type entityType = null!;
        
        // Act
        var action = () => ((IObjectStoreFactory)factory).GetObjectStore(database, entityType);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("entityType", exception.ParamName);
    }

    [Fact]
    public void GetObjectStore_WhenCalled_CreatesObjectStore()
    {
        // Arrange
        var expressionBuilder = new Mock<IExpressionBuilder>().Object;
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var factory = new ObjectStoreFactory(expressionBuilder, jsRuntime);
        var database = new IndexedDbDatabase(new MockIndexedDbDatabase(factory), jsRuntime);
        var entityType = typeof(object);
     
        // Act
        var objectStore = ((IObjectStoreFactory)factory).GetObjectStore(database, entityType);
        
        // Assert
        Assert.NotNull(objectStore);
        Assert.IsType<ObjectStore<object>>(objectStore);
    }
}