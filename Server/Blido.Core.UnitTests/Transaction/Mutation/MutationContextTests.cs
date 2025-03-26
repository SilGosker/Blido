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
        IServiceScope serviceScope = null!;

        var database = new MockIndexedDbDatabase(new ObjectStoreFactory(new Mock<IExpressionBuilder>().Object,
            new Mock<IJSRuntime>().Object));

        // Act
        Action act = () => new MutationContext(new OptionsWrapper<MutationConfiguration>(new MutationConfiguration()), serviceScope, database);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("serviceScope", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenPipelineTypesIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var serviceScope = new Mock<IServiceScope>().Object;
        IOptions<MutationConfiguration> mutationConfiguration = null!;
        var database = new MockIndexedDbDatabase(new ObjectStoreFactory(new Mock<IExpressionBuilder>().Object,
            new Mock<IJSRuntime>().Object));

        // Act
        Action act = () => new MutationContext(mutationConfiguration, serviceScope, database);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("mutationConfiguration", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenContextIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var serviceScope = new Mock<IServiceScope>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        IndexedDbContext context = null!;

        // Act
        Action act = () => new MutationContext(options, serviceScope, context);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("context", exception.ParamName);
    }

    [Fact]
    public void Constructor_InitializesEntities()
    {
        // Arrange
        var serviceScope = new Mock<IServiceScope>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var database = new MockIndexedDbDatabase(new ObjectStoreFactory(new Mock<IExpressionBuilder>().Object,
            new Mock<IJSRuntime>().Object));

        // Act
        var context = new MutationContext(options, serviceScope, database);
        
        // Assert
        Assert.NotNull(context.Entities);
        Assert.Empty(context.Entities);
    }

    [Fact]
    public void Constructor_SetsDatabaseProperty()
    {
        // Arrange
        var serviceScope = new Mock<IServiceScope>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var database = new MockIndexedDbDatabase(new ObjectStoreFactory(new Mock<IExpressionBuilder>().Object,
            new Mock<IJSRuntime>().Object));

        // Act
        var context = new MutationContext(options, serviceScope, database);

        // Assert
        Assert.NotNull(context.Database);
        Assert.Same(database, context.Database);
    }

    [Fact]
    public void Insert_AddsMutationEntityContext_ToEntities()
    {
        // Arrange
        var serviceScope = new Mock<IServiceScope>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var database = new MockIndexedDbDatabaseWithObjectStoreSetProperties(new ObjectStoreFactory(new Mock<IExpressionBuilder>().Object,
            new Mock<IJSRuntime>().Object));
        var context = new MutationContext(options, serviceScope, database);
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
        var serviceScope = new Mock<IServiceScope>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var database = new MockIndexedDbDatabaseWithObjectStoreSetProperties(new ObjectStoreFactory(new Mock<IExpressionBuilder>().Object,
            new Mock<IJSRuntime>().Object));
        var context = new MutationContext(options, serviceScope, database);
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
        var serviceScope = new Mock<IServiceScope>().Object;
        var options = new OptionsWrapper<MutationConfiguration>(new MutationConfiguration());
        var database = new MockIndexedDbDatabaseWithObjectStoreSetProperties(new ObjectStoreFactory(new Mock<IExpressionBuilder>().Object,
            new Mock<IJSRuntime>().Object));
        var context = new MutationContext(options, serviceScope, database);
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
        var serviceScope = new Mock<IServiceScope>();
        serviceScope.SetupGet(x => x.ServiceProvider).Returns(new Mock<IServiceProvider>().Object);
        var config = new MutationConfiguration();
        config.Use(typeof(MockBlidoMiddleware));
        config.Use(typeof(MockBlidoMiddleware)); 
        var options = new OptionsWrapper<MutationConfiguration>(config);
        var database = new MockIndexedDbDatabase(new ObjectStoreFactory(new Mock<IExpressionBuilder>().Object,
            new Mock<IJSRuntime>().Object));
        var context = new MutationContext(options, serviceScope.Object, database);

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
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(mockBlidoMiddleware);
        var config = new MutationConfiguration();
        config.Use(typeof(MockBlidoMiddleware));
        config.Use(typeof(MockBlidoMiddleware));
        var options = new OptionsWrapper<MutationConfiguration>(config);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var mockServiceScope = new Mock<IServiceScope>();
        mockServiceScope.SetupGet(x => x.ServiceProvider).Returns(serviceProvider);
        var database = new MockIndexedDbDatabase(new ObjectStoreFactory(new Mock<IExpressionBuilder>().Object,
            new Mock<IJSRuntime>().Object));
        var context = new MutationContext(options, mockServiceScope.Object, database);

        // Act
        await context.SaveChangesAsync();

        // Assert
        Assert.Equal(2, mockBlidoMiddleware.TimesCalled);
    }
}