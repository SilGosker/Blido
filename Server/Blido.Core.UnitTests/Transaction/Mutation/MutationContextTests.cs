using System.Linq;
using Blido.Core.Options;
using Blido.Core.Transaction.JsExpression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Moq;

namespace Blido.Core.Transaction.Mutation;

public class MutationContextTests
{
    [Fact]
    public void Constructor_WhenQueryProviderIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IServiceProvider serviceProvider = null!;
        var mockObjectStoreFactory = new Mock<IObjectStoreFactory>();
        mockObjectStoreFactory.SetupGet(x => x.JsRuntime).Returns(new Mock<IJSRuntime>().Object);
        var context = new MockIndexedDbDatabase(mockObjectStoreFactory.Object);

        // Act
        Action act = () => new MutationContext(new OptionsWrapper<MutationConfiguration>(new MutationConfiguration()), serviceProvider, context);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("serviceProvider", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenPipelineTypesIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var serviceProvider = new Mock<IServiceProvider>().Object;
        IOptions<MutationConfiguration> mutationConfiguration = null!;
        var mockObjectStoreFactory = new Mock<IObjectStoreFactory>();
        mockObjectStoreFactory.SetupGet(x => x.JsRuntime).Returns(new Mock<IJSRuntime>().Object);
        var context = new MockIndexedDbDatabase(mockObjectStoreFactory.Object);

        // Act
        Action act = () => new MutationContext(mutationConfiguration, serviceProvider, context);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("mutationConfiguration", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenContextIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        IndexedDbContext context = null!;

        // Act
        Action act = () => new MutationContext(options, serviceProvider, context);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("context", exception.ParamName);
    }

    [Fact]
    public void Constructor_InitializesEntities()
    {
        // Arrange
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var mockObjectStoreFactory = new Mock<IObjectStoreFactory>();
        mockObjectStoreFactory.SetupGet(x => x.JsRuntime).Returns(new Mock<IJSRuntime>().Object);
        var dbContext = new MockIndexedDbDatabase(mockObjectStoreFactory.Object);

        // Act
        var context = new MutationContext(options, serviceProvider, dbContext);
        
        // Assert
        Assert.NotNull(context.Entities);
        Assert.Empty(context.Entities);
    }

    [Fact]
    public void Constructor_SetsDatabaseProperty()
    {
        // Arrange
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var mockObjectStoreFactory = new Mock<IObjectStoreFactory>();
        mockObjectStoreFactory.SetupGet(x => x.JsRuntime).Returns(new Mock<IJSRuntime>().Object);
        var dbContext = new MockIndexedDbDatabase(mockObjectStoreFactory.Object);

        // Act
        var context = new MutationContext(options, serviceProvider, dbContext);

        // Assert
        Assert.NotNull(context.Database);
        Assert.Same(dbContext.Database, context.Database);
    }

    [Fact]
    public void Insert_AddsMutationEntityContext_ToEntities()
    {
        // Arrange
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var mockObjectStoreFactory = new Mock<IObjectStoreFactory>();
        mockObjectStoreFactory.SetupGet(x => x.JsRuntime).Returns(new Mock<IJSRuntime>().Object);
        var dbContext = new MockIndexedDbDatabaseWithObjectStoreSetProperties(mockObjectStoreFactory.Object);
        var context = new MutationContext(options, serviceProvider, dbContext);
        var entity = new object();

        // Act
        context.Insert(entity);
        
        // Assert
        Assert.Single(context.Entities);
        Assert.Same(entity, context.Entities[0].BeforeChange);
        Assert.Equal(MutationState.Added, context.Entities[0].State);
    }

    [Fact]
    public void Update_AddsMutationEntityContext_ToEntities()
    {
        // Arrange
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var mockObjectStoreFactory = new Mock<IObjectStoreFactory>();
        mockObjectStoreFactory.SetupGet(x => x.JsRuntime).Returns(new Mock<IJSRuntime>().Object);
        var dbContext = new MockIndexedDbDatabaseWithObjectStoreSetProperties(mockObjectStoreFactory.Object);
        var context = new MutationContext(options, serviceProvider, dbContext);
        var entity = new object();

        // Act
        context.Update(entity);

        // Assert
        Assert.Single(context.Entities);
        Assert.Same(entity, context.Entities[0].BeforeChange);
        Assert.Equal(MutationState.Modified, context.Entities[0].State);
    }

    [Fact]
    public void Delete_AddsMutationEntityContext_ToEntities()
    {
        // Arrange
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var mockObjectStoreFactory = new Mock<IObjectStoreFactory>();
        mockObjectStoreFactory.SetupGet(x => x.JsRuntime).Returns(new Mock<IJSRuntime>().Object);
        var dbContext = new MockIndexedDbDatabaseWithObjectStoreSetProperties(mockObjectStoreFactory.Object);
        var context = new MutationContext(options, serviceProvider, dbContext);
        var entity = new object();

        // Act
        context.Delete(entity);
        
        // Assert
        Assert.Single(context.Entities);
        Assert.Same(entity, context.Entities[0].BeforeChange);
        Assert.Equal(MutationState.Deleted, context.Entities[0].State);
    }

    [Fact]
    public async Task SaveChangesAsync_InvokesMiddleware()
    {
        // Arrange
        MockBlidoMiddleware.TotalTimesCalled = 0;
        var serviceCollection = new ServiceCollection();
        var config = new MutationConfiguration();
        config.Use(typeof(MockBlidoMiddleware));
        config.Use(typeof(MockBlidoMiddleware)); 
        var options = new OptionsWrapper<MutationConfiguration>(config);
        var mockObjectStoreFactory = new Mock<IObjectStoreFactory>();
        mockObjectStoreFactory.SetupGet(x => x.JsRuntime).Returns(new Mock<IJSRuntime>().Object);
        var dbContext = new MockIndexedDbDatabaseWithObjectStoreSetProperties(mockObjectStoreFactory.Object);
        var context = new MutationContext(options, serviceCollection.BuildServiceProvider(), dbContext);
        context.Insert(new object());

        // Act
        await context.SaveChangesAsync();

        // Assert
        Assert.Equal(2, MockBlidoMiddleware.TotalTimesCalled);
    }

    [Fact]
    public async Task SaveChangesAsync_GetsServiceFromDependencyContainer()
    {
        // Arrange
        var mockBlidoMiddleware = new MockBlidoMiddleware();
        var serviceCollection = new ServiceCollection().AddSingleton(mockBlidoMiddleware);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var mockObjectStoreFactory = new Mock<IObjectStoreFactory>();
        mockObjectStoreFactory.SetupGet(x => x.JsRuntime).Returns(new Mock<IJSRuntime>().Object);
        var dbContext = new MockIndexedDbDatabaseWithObjectStoreSetProperties(mockObjectStoreFactory.Object);

        var config = new MutationConfiguration();
        config.Use(typeof(MockBlidoMiddleware));
        config.Use(typeof(MockBlidoMiddleware));
        var options = new OptionsWrapper<MutationConfiguration>(config);
        var context = new MutationContext(options, serviceProvider, dbContext);
        context.Insert(new object());
        // Act
        await context.SaveChangesAsync();

        // Assert
        Assert.Equal(2, mockBlidoMiddleware.TimesCalled);
    }
}