using System.Linq;
using Microsoft.Extensions.DependencyInjection;
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

        // Act
        Action act = () => new MutationContext(pipelineTypes, serviceScope);

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
        
        // Act
        Action act = () => new MutationContext(pipelineTypes, serviceScope);
        
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

        // Act
        var context = new MutationContext(pipelineTypes, serviceScope);
        
        // Assert
        Assert.NotNull(context.Entities);
        Assert.Empty(context.Entities);
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
        var context = new MutationContext(pipelineTypes, serviceScope.Object);

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
        var context = new MutationContext(pipelines, mockServiceScope.Object);

        // Act
        await context.SaveChangesAsync();

        // Assert
        Assert.Equal(2, mockBlidoMiddleware.TimesCalled);
    }
}