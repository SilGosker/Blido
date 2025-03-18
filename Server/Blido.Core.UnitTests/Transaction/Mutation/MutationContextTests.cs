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
}