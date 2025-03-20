using System.Linq;
using Blido.Core.Transaction.JsExpression;
using Microsoft.Extensions.DependencyInjection;
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
        var pipelineTypes = new List<Type>();
        var database = new MockIndexedDbDatabase(new ObjectStoreFactory(new Mock<IExpressionBuilder>().Object,
            new Mock<IJSRuntime>().Object));

        // Act
        Action act = () => new MutationContext(pipelineTypes, serviceScope, database);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("serviceScope", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenPipelineTypesIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var serviceScope = new Mock<IServiceScope>().Object;
        List<Type> pipelineTypes = null!;
        var database = new MockIndexedDbDatabase(new ObjectStoreFactory(new Mock<IExpressionBuilder>().Object,
            new Mock<IJSRuntime>().Object));

        // Act
        Action act = () => new MutationContext(pipelineTypes, serviceScope, database);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("pipelineTypes", exception.ParamName);
    }

    [Fact]
    public void Constructor_InitializesEntities()
    {
        // Arrange
        var serviceScope = new Mock<IServiceScope>().Object;
        var pipelineTypes = new List<Type>();
        var database = new MockIndexedDbDatabase(new ObjectStoreFactory(new Mock<IExpressionBuilder>().Object,
            new Mock<IJSRuntime>().Object));

        // Act
        var context = new MutationContext(pipelineTypes, serviceScope, database);
        
        // Assert
        Assert.NotNull(context.Entities);
        Assert.Empty(context.Entities);
    }

    [Fact]
    public void Insert_AddsMutationEntityContext_ToEntities()
    {
        // Arrange
        var serviceScope = new Mock<IServiceScope>().Object;
        var pipelineTypes = new List<Type>();
        var database = new MockIndexedDbDatabase(new ObjectStoreFactory(new Mock<IExpressionBuilder>().Object,
            new Mock<IJSRuntime>().Object));
        var context = new MutationContext(pipelineTypes, serviceScope, database);
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
        var pipelineTypes = new List<Type>();
        var database = new MockIndexedDbDatabase(new ObjectStoreFactory(new Mock<IExpressionBuilder>().Object,
            new Mock<IJSRuntime>().Object));
        var context = new MutationContext(pipelineTypes, serviceScope, database);
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
        var pipelineTypes = new List<Type>();
        var database = new MockIndexedDbDatabase(new ObjectStoreFactory(new Mock<IExpressionBuilder>().Object,
            new Mock<IJSRuntime>().Object));
        var context = new MutationContext(pipelineTypes, serviceScope, database);
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
        var pipelineTypes = new List<Type>()
        {
            typeof(MockBlidoMiddleware),
            typeof(MockBlidoMiddleware)
        };
        var database = new MockIndexedDbDatabase(new ObjectStoreFactory(new Mock<IExpressionBuilder>().Object,
            new Mock<IJSRuntime>().Object));
        var context = new MutationContext(pipelineTypes, serviceScope.Object, database);

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
        var pipelines = new List<Type>() { typeof(MockBlidoMiddleware), typeof(MockBlidoMiddleware) };
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var mockServiceScope = new Mock<IServiceScope>();
        mockServiceScope.SetupGet(x => x.ServiceProvider).Returns(serviceProvider);
        var database = new MockIndexedDbDatabase(new ObjectStoreFactory(new Mock<IExpressionBuilder>().Object,
            new Mock<IJSRuntime>().Object));
        var context = new MutationContext(pipelines, mockServiceScope.Object, database);

        // Act
        await context.SaveChangesAsync();

        // Assert
        Assert.Equal(2, mockBlidoMiddleware.TimesCalled);
    }
}