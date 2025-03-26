using Blido.Core.Options;
using Blido.Core.Transaction.JsExpression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Moq;

namespace Blido.Core.Transaction;

public class QueryProviderTests
{
    [Fact]
    public void Constructor_WithNullJsRuntime_ShouldThrowArgumentNullException()
    {
        // Arrange
        IJSRuntime jsRuntime = null!;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.Setup(x => x.JsRuntime).Returns(new Mock<IJSRuntime>().Object);
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        
        // Act
        Action act = () => new QueryProvider<object>(jsRuntime, jsExpressionBuilder);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("jsRuntime", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullJsExpressionBuilder_ShouldThrowArgumentNullException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.Setup(x => x.JsRuntime).Returns(jsRuntime);
        IExpressionBuilder jsExpressionBuilder = null!;

        // Act
        Action act = () => new QueryProvider<object>(jsRuntime, jsExpressionBuilder);

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
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        var queryProvider = new QueryProvider<object>(jsRuntime, jsExpressionBuilder);

        // Act
        Action act = () => queryProvider.Where(null!);
        
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
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        var queryProvider = new QueryProvider<object>(jsRuntime, jsExpressionBuilder);

        // Act
        Func<Task> act = async () => await queryProvider.ExecuteAsync<object>(null!, CancellationToken.None);
        
        // Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(act);
        Assert.Equal("methodName", exception.ParamName);
    }

    [Fact]
    public async Task ExecuteAsync_WithUnsupportedMethodName_ShouldThrowArgumentException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.Setup(x => x.JsRuntime).Returns(jsRuntime);
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        var queryProvider = new QueryProvider<object>(jsRuntime, jsExpressionBuilder);

        // Act
        Func<Task> act = async () => await queryProvider.ExecuteAsync<object>("unsupportedMethodName", CancellationToken.None);

        // Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(act);
        Assert.Equal("Method name 'unsupportedMethodName' is not supported.", exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidArguments_ShouldNotThrow()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>();
        jsRuntime.Setup(x => x.InvokeAsync<ulong>(It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<object[]>())).ReturnsAsync(1ul);
        jsRuntime.Setup(x => x.InvokeAsync<bool>(It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<object[]>())).ReturnsAsync(true);
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.Setup(x => x.JsRuntime).Returns(jsRuntime.Object);
        var context = new MockIndexedDbDatabase(objectStoreFactory.Object);
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        var queryProvider = new QueryProvider<object>(jsRuntime.Object, jsExpressionBuilder);
        ((IQueryProvider)queryProvider).SetObjectStore(new ObjectStore<object>(context, queryProvider, new Mock<IServiceProvider>().Object, new OptionsWrapper<MutationConfiguration>(new MutationConfiguration())));

        // Act
        var result = await queryProvider.ExecuteAsync<bool>("AnyAsync");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExecuteAsync_SelectorOverload_WithNullMethodName_ShouldThrowArgumentNullException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.Setup(x => x.JsRuntime).Returns(jsRuntime);
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        var queryProvider = new QueryProvider<object>(jsRuntime, jsExpressionBuilder);

        // Act
        Func<Task> act = async () => await queryProvider.ExecuteAsync<object>(null!, new Func<object, object>(x => x), CancellationToken.None);

        // Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(act);
        Assert.Equal("methodName", exception.ParamName);
    }

    [Fact]
    public async Task ExecuteAsync_SelectorOverload_WithUnsupportedMethodName_ShouldThrowArgumentException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.Setup(x => x.JsRuntime).Returns(jsRuntime);
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        var queryProvider = new QueryProvider<object>(jsRuntime, jsExpressionBuilder);

        // Act
        Func<Task> act = async () => await queryProvider.ExecuteAsync<object>("unsupportedMethodName", new Func<object, object>(x => x), CancellationToken.None);

        // Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(act);
        Assert.Equal("Method name 'unsupportedMethodName' is not supported.", exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_SelectorOverload_WithNullSelector_ShouldThrowArgumentNullException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.Setup(x => x.JsRuntime).Returns(jsRuntime);
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        var queryProvider = new QueryProvider<object>(jsRuntime, jsExpressionBuilder);

        // Act
        Func<Task> act = async () => await queryProvider.ExecuteAsync<object>("methodName", null!, CancellationToken.None);
        
        // Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(act);
        Assert.Equal("selector", exception.ParamName);
    }

    [Fact]
    public async Task ExecuteAsync_SelectorOverload_WithValidArguments_ShouldNotThrow()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>();
        jsRuntime.Setup(x => x.InvokeAsync<object>(It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<object[]>())).ReturnsAsync(new object());
        jsRuntime.Setup(x => x.InvokeAsync<ulong>(It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<object[]>())).ReturnsAsync(1ul);
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.Setup(x => x.JsRuntime).Returns(jsRuntime.Object);
        var context = new MockIndexedDbDatabase(objectStoreFactory.Object);
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        var queryProvider = new QueryProvider<object>(jsRuntime.Object, jsExpressionBuilder);
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        ((IQueryProvider)queryProvider).SetObjectStore(new ObjectStore<object>(context, queryProvider, serviceProvider, options));

        // Act
        var result = await queryProvider.ExecuteAsync<object>("SumAsync", selector: _ => 1, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task ExecuteAsync_IdentifiersOverload_WithNullMethodName_ShouldThrowArgumentNullException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.Setup(x => x.JsRuntime).Returns(jsRuntime);
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        var queryProvider = new QueryProvider<object>(jsRuntime, jsExpressionBuilder);

        // Act
        Func<Task> act = async () => await queryProvider.ExecuteAsync<object>(null!, new { }, CancellationToken.None);
        
        // Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(act);
        Assert.Equal("methodName", exception.ParamName);
    }

    [Fact]
    public async Task ExecuteAsync_IdentifiersOverload_WithUnsupportedMethodName_ShouldThrowArgumentException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.Setup(x => x.JsRuntime).Returns(jsRuntime);
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        var queryProvider = new QueryProvider<object>(jsRuntime, jsExpressionBuilder);

        // Act
        Func<Task> act = async () => await queryProvider.ExecuteAsync<object>("unsupportedMethodName", new { }, CancellationToken.None);
        
        // Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(act);
        Assert.Equal("Method name 'unsupportedMethodName' is not supported.", exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_IdentifiersOverload_WithNullIdentifiers_ShouldThrowArgumentNullException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.Setup(x => x.JsRuntime).Returns(jsRuntime);
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        var queryProvider = new QueryProvider<object>(jsRuntime, jsExpressionBuilder);

        // Act
        Func<Task> act = async () => await queryProvider.ExecuteAsync<object>("methodName", identifiers: null!, CancellationToken.None);
        
        // Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(act);
        Assert.Equal("identifiers", exception.ParamName);
    }

    [Fact]
    public async Task ExecuteAsync_IdentifiersOverload_WithValidArguments_ShouldNotThrow()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>();
        jsRuntime.Setup(x => x.InvokeAsync<object>(It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<object[]>())).ReturnsAsync(new object());
        jsRuntime.Setup(x => x.InvokeAsync<ulong>(It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<object[]>())).ReturnsAsync(1ul);
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.Setup(x => x.JsRuntime).Returns(jsRuntime.Object);
        var context = new MockIndexedDbDatabase(objectStoreFactory.Object);
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        var queryProvider = new QueryProvider<object>(jsRuntime.Object, jsExpressionBuilder);
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        ((IQueryProvider)queryProvider).SetObjectStore(new ObjectStore<object>(context, queryProvider, serviceProvider, options));

        // Act
        var result = await queryProvider.ExecuteAsync<object>("FindAsync", new object(), CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task ExecuteAsync_NonGenericArgumentOverload_WithNullMethodName_ShouldThrowArgumentNullException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.Setup(x => x.JsRuntime).Returns(jsRuntime);
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        var queryProvider = new QueryProvider<object>(jsRuntime, jsExpressionBuilder);

        // Act
        Func<Task> act = async () => await queryProvider.ExecuteAsync(null!, CancellationToken.None);

        // Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(act);
        Assert.Equal("methodName", exception.ParamName);
    }

    [Fact]
    public async Task ExecuteAsync_NonGenericArgumentOverload_WithUnsupportedMethodName_ShouldThrowArgumentException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.Setup(x => x.JsRuntime).Returns(jsRuntime);
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        var queryProvider = new QueryProvider<object>(jsRuntime, jsExpressionBuilder);

        // Act
        Func<Task> act = async () => await queryProvider.ExecuteAsync("unsupportedMethodName", CancellationToken.None);
        
        // Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(act);
        Assert.Equal("Method name 'unsupportedMethodName' is not supported.", exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_NonGenericArgumentOverload_WithValidArguments_ShouldNotThrow()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>();
        jsRuntime.Setup(x => x.InvokeAsync<object>(It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<object[]>())).ReturnsAsync(new object());
        jsRuntime.Setup(x => x.InvokeAsync<ulong>(It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<object[]>())).ReturnsAsync(1ul);
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.Setup(x => x.JsRuntime).Returns(jsRuntime.Object);
        var context = new MockIndexedDbDatabase(objectStoreFactory.Object);
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        var queryProvider = new QueryProvider<object>(jsRuntime.Object, jsExpressionBuilder);
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        ((IQueryProvider)queryProvider).SetObjectStore(new ObjectStore<object>(context, queryProvider, serviceProvider, options));

        // Act
        await queryProvider.ExecuteAsync("CountAsync", CancellationToken.None);
    }


}