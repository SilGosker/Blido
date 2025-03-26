using System.Linq.Expressions;
using Blido.Core.Options;
using Blido.Core.Transaction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Moq;
using Xunit.Sdk;

namespace Blido.Core;

public class ObjectStoreTests
{
    [Fact]
    public void Constructor_WithNullProvider_ThrowsArgumentNullException()
    {
        // Arrange
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        var jsRuntime = new Mock<IJSRuntime>().Object;
        objectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntime);
        ITransactionProvider<object> provider = null!;
        var context = new MockIndexedDbDatabase(objectStoreFactory.Object);
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());

        // Act
        Action act = () => new ObjectStore<object>(context, provider, serviceProvider, options);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("provider", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullDatabase_ThrowsArgumentNullException()
    {
        // Arrange
        var provider = new Mock<ITransactionProvider<object>>().Object;
        IndexedDbContext context = null!;
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());

        // Act
        Action act = () => new ObjectStore<object>(context, provider, serviceProvider, options);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("context", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullServiceProvider_ThrowsArgumentNullException()
    {
        // Arrange
        var provider = new Mock<ITransactionProvider<object>>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        var jsRuntime = new Mock<IJSRuntime>().Object;
        objectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntime);
        var context = new MockIndexedDbDatabase(objectStoreFactory.Object);
        IServiceProvider serviceProvider = null!;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());

        // Act
        Action act = () => new ObjectStore<object>(context, provider, serviceProvider, options);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("serviceProvider", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullOptions_ThrowsArgumentNullException()
    {
        // Arrange
        var provider = new Mock<ITransactionProvider<object>>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        var jsRuntime = new Mock<IJSRuntime>().Object;
        objectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntime);
        var context = new MockIndexedDbDatabase(objectStoreFactory.Object);
        var serviceProvider = new Mock<IServiceProvider>().Object;
        IOptions<MutationConfiguration> options = null!;

        // Act
        Action act = () => new ObjectStore<object>(context, provider, serviceProvider, options);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("options", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithValidProvider_SetsName()
    {
        // Arrange
        var provider = new Mock<ITransactionProvider<object>>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        var jsRuntime = new Mock<IJSRuntime>().Object;
        objectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntime);
        var context = new MockIndexedDbDatabase(objectStoreFactory.Object);
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());

        // Act
        var objectStoreSet = new ObjectStore<object>(context, provider, serviceProvider, options);

        // Assert
        Assert.Equal("Object", objectStoreSet.Name);
    }

    [Fact]
    public void Constructor_WithValidProvider_SetsDatabase()
    {
        // Arrange
        var provider = new Mock<ITransactionProvider<object>>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        var jsRuntime = new Mock<IJSRuntime>().Object;
        objectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntime);
        var context = new MockIndexedDbDatabase(objectStoreFactory.Object);
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());

        // Act
        var objectStoreSet = new ObjectStore<object>(context, provider, serviceProvider, options);
        
        // Assert
        Assert.Same(context, objectStoreSet.Context);
        Assert.Same(context.Database, objectStoreSet.Context.Database);
    }

    [Fact]
    public void Where_WithNull_ThrowsArgumentNullException()
    {
        // Arrange
        var provider = new Mock<ITransactionProvider<object>>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        var jsRuntime = new Mock<IJSRuntime>().Object;
        objectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntime);
        var context = new MockIndexedDbDatabase(objectStoreFactory.Object);
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var objectStoreSet = new ObjectStore<object>(context, provider, serviceProvider, options);

        // Act
        Action act = () => objectStoreSet.Where(null!);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("expression", exception.ParamName);
    }

    [Fact]
    public void Where_WithExpression_CallsWhereOnTransactionProvider()
    {
        // Arrange
        var providerMock = new Mock<ITransactionProvider<object>>();
        var provider = providerMock.Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        var jsRuntime = new Mock<IJSRuntime>().Object;
        objectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntime);
        var context = new MockIndexedDbDatabase(objectStoreFactory.Object);
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var objectStoreSet = new ObjectStore<object>(context, provider, serviceProvider, options);
        Expression<Func<object, bool>> expression = _ => true;

        // Act
        objectStoreSet.Where(expression);

        // Assert
        providerMock.Verify(x => x.Where(expression), Times.Once);
    }

    [Fact]
    public void Where_WithExpression_ReturnsInstance()
    {
        // Arrange
        var provider = new Mock<ITransactionProvider<object>>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        var jsRuntime = new Mock<IJSRuntime>().Object;
        objectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntime);
        IndexedDbContext database = new MockIndexedDbDatabase(objectStoreFactory.Object);
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var objectStoreSet = new ObjectStore<object>(database, provider, serviceProvider, options);
        Expression<Func<object, bool>> expression = _ => true;

        // Act
        var result = objectStoreSet.Where(expression);
        
        // Assert
        Assert.Same(objectStoreSet, result);
    }
}