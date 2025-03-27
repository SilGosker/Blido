using Blido.Core.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Moq;
using Xunit.Sdk;

namespace Blido.Core.Transaction.Mutation.Arguments;

public class MutateBatchArgumentsTests
{
    [Fact]
    public async Task CreateAsync_WhenContextIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        MutationContext context = null!;
        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        async Task Act() => await MutateBatchArguments.CreateAsync(context, cancellationToken);
        
        // Assert
        ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(Act);
        Assert.Equal("context", exception.ParamName);
    }

    [Fact]
    public async Task CreateAsync_WithContext_SetsVersionAndDatabaseName()
    {
        // Arrange
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var jsRuntime = new Mock<IJSRuntime>();
        jsRuntime.Setup(x =>
            x.InvokeAsync<ulong>(It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<object[]>())).ReturnsAsync(1ul);
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntime.Object);
        var context = new MutationContext(options, serviceProvider, new MockIndexedDbDatabaseWithAttribute(objectStoreFactory.Object));
        
        // Act
        MutateBatchArguments arguments = await MutateBatchArguments.CreateAsync(context, CancellationToken.None);
        
        // Assert
        Assert.Equal("CustomName", arguments.Database);
        Assert.Equal(1ul, arguments.Version);
    }

    [Fact]
    public async Task CreateAsync_WithContextWithAttribute_SetsVersionAndDatabaseName()
    {
        // Arrange
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var jsRuntime = new Mock<IJSRuntime>();
        jsRuntime.Setup(x =>
            x.InvokeAsync<ulong>(It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<object[]>())).ReturnsAsync(1ul);
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntime.Object);
        var context = new MutationContext(options, serviceProvider, new MockIndexedDbDatabaseWithAttribute(objectStoreFactory.Object));

        // Act
        MutateBatchArguments arguments = await MutateBatchArguments.CreateAsync(context, CancellationToken.None);

        // Assert
        Assert.Equal("CustomName", arguments.Database);
        Assert.Equal(1ul, arguments.Version);
    }

    [Theory]
    [InlineData("test", typeof(object))]
    [InlineData(typeof(object), "test")]
    [InlineData("test", typeof(object), "test")]
    [InlineData(typeof(object), "test", typeof(object))]
    public async Task CreateAsync_WithEntities_GroupsIntoMutateBatchObjectStoreArguments(params object[] entities)
    {
        // Arrange
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var jsRuntime = new Mock<IJSRuntime>();
        jsRuntime.Setup(x =>
            x.InvokeAsync<ulong>(It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<object[]>())).ReturnsAsync(1ul);
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntime.Object);
        var context = new MutationContext(options, serviceProvider, new MockIndexedDbDatabaseWithObjectStoreSetProperties(objectStoreFactory.Object));
        foreach (object entity in entities)
        {
            if (entity is Type)
            {
                context.Insert(new object());
                continue;
            }
            context.Insert(entity);
        }

        // Act
        MutateBatchArguments arguments = await MutateBatchArguments.CreateAsync(context, CancellationToken.None);

        // Assert
        Assert.Equal("MockIndexedDbDatabaseWithObjectStoreSetProperties", arguments.Database);
        Assert.Equal(2, arguments.ObjectStores.Count);
    }

    [Fact]
    public async Task CreateAsync_WithEntities_SerializesEntities()
    {
        // Arrange
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var jsRuntime = new Mock<IJSRuntime>();
        jsRuntime.Setup(x =>
            x.InvokeAsync<ulong>(It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<object[]>())).ReturnsAsync(1ul);
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntime.Object);
        var context = new MutationContext(options, serviceProvider, new MockIndexedDbDatabaseWithObjectStoreSetProperties(objectStoreFactory.Object));
        context.Insert("first");
        context.Insert("second");

        // Act
        MutateBatchArguments arguments = await MutateBatchArguments.CreateAsync(context, CancellationToken.None);
        
        // Assert
        Assert.Equal("\"first\"", arguments.ObjectStores[0].Entities[0].Entity);
        Assert.Equal("\"second\"", arguments.ObjectStores[0].Entities[1].Entity);
    }

    [Fact]
    public async Task CreateAsync_WithValidObjectStoreEntity_SetsObjectStoreName()
    {
        // Arrange
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var jsRuntime = new Mock<IJSRuntime>();
        jsRuntime.Setup(x =>
            x.InvokeAsync<ulong>(It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<object[]>())).ReturnsAsync(1ul);
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntime.Object);
        var context = new MutationContext(options, serviceProvider, new MockIndexedDbDatabaseWithAttributeAndObjectStoreSetProperties(objectStoreFactory.Object));
        context.Insert(new MockObjectStoreWithAttribute());

        // Act
        MutateBatchArguments arguments = await MutateBatchArguments.CreateAsync(context, CancellationToken.None);

        // Assert
        Assert.Equal("CustomName", arguments.ObjectStores[0].ObjectStore);
    }

    [Fact]
    public async Task CreateAsync_WithObjects_SetsState()
    {
        // Arrange
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var jsRuntime = new Mock<IJSRuntime>();
        jsRuntime.Setup(x =>
            x.InvokeAsync<ulong>(It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<object[]>())).ReturnsAsync(1ul);
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntime.Object);
        var context = new MutationContext(options, serviceProvider, new MockIndexedDbDatabaseWithObjectStoreSetProperties(objectStoreFactory.Object));
        context.Insert("first");
        context.Update("second");

        // Act
        MutateBatchArguments arguments = await MutateBatchArguments.CreateAsync(context, CancellationToken.None);

        // Assert
        Assert.Equal(MutationState.Added, arguments.ObjectStores[0].Entities[0].State);
        Assert.Equal(MutationState.Modified, arguments.ObjectStores[0].Entities[1].State);
    }

    [Fact]
    public async Task Properties_ShouldSetValues()
    {
        // Arrange
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var jsRuntime = new Mock<IJSRuntime>();
        jsRuntime.Setup(x =>
            x.InvokeAsync<ulong>(It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<object[]>())).ReturnsAsync(1ul);
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntime.Object);
        var context = new MutationContext(options, serviceProvider, new MockIndexedDbDatabaseWithAttributeAndObjectStoreSetProperties(objectStoreFactory.Object));
        context.Insert(new MockObjectStoreWithAttribute());
        MutateBatchArguments arguments = await MutateBatchArguments.CreateAsync(context, CancellationToken.None);

        // Act
        arguments.Database = "";
        arguments.Version = 20ul;
        arguments.ObjectStores = null!;

        // Assert
        Assert.Equal("", arguments.Database);
        Assert.Equal(20ul, arguments.Version);
        Assert.Null(arguments.ObjectStores);
    }
}