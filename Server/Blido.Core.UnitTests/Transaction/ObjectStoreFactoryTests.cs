using Blido.Core.Options;
using Blido.Core.Transaction.JsExpression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
        var serviceProvider = new Mock<IServiceProvider>();
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        
        // Act
        var action = () => new ObjectStoreFactory(expressionBuilder, jsRuntime, serviceProvider.Object, options);
        
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
        var serviceProvider = new Mock<IServiceProvider>();
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());

        // Act
        var action = () => new ObjectStoreFactory(expressionBuilder, jsRuntime, serviceProvider.Object, options);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("jsRuntime", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenServiceProviderIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var expressionBuilder = new Mock<IExpressionBuilder>().Object;
        var jsRuntime = new Mock<IJSRuntime>().Object;
        IServiceProvider serviceProvider = null!;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());

        // Act
        var action = () => new ObjectStoreFactory(expressionBuilder, jsRuntime, serviceProvider, options);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("serviceProvider", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenOptionsIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var expressionBuilder = new Mock<IExpressionBuilder>().Object;
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var serviceProvider = new Mock<IServiceProvider>();
        IOptions<MutationConfiguration> options = null!;

        // Act
        var action = () => new ObjectStoreFactory(expressionBuilder, jsRuntime, serviceProvider.Object, options);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("options", exception.ParamName);
    }

    [Fact]
    public void GetObjectStore_WhenDatabaseIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var expressionBuilder = new Mock<IExpressionBuilder>().Object;
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var serviceProvider = new Mock<IServiceProvider>();
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var factory = new ObjectStoreFactory(expressionBuilder, jsRuntime, serviceProvider.Object, options);
        IndexedDbContext database = null!;
        var entityType = typeof(object);

        // Act
        var action = () => ((IObjectStoreFactory)factory).GetObjectStore(database, entityType);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("context", exception.ParamName);
    }

    [Fact]
    public void GetObjectStore_WhenEntityTypeIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var expressionBuilder = new Mock<IExpressionBuilder>().Object;
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var serviceProvider = new Mock<IServiceProvider>();
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var factory = new ObjectStoreFactory(expressionBuilder, jsRuntime, serviceProvider.Object, options);
        var database = new MockIndexedDbDatabase(factory);
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
        var serviceProvider = new Mock<IServiceProvider>();
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var factory = new ObjectStoreFactory(expressionBuilder, jsRuntime, serviceProvider.Object, options);
        var database = new MockIndexedDbDatabase(factory);
        var entityType = typeof(object);
     
        // Act
        var objectStore = ((IObjectStoreFactory)factory).GetObjectStore(database, entityType);
        
        // Assert
        Assert.NotNull(objectStore);
        Assert.IsType<ObjectStore<object>>(objectStore);
    }
}