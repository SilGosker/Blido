using System.Linq;
using Moq;

namespace Blido.Core.Transaction.Mutation;

public class MutationContextTests
{
    [Fact]
    public void Constructor_WhenQueryProviderIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        ITransactionProvider<object> queryProvider = null!;
        var pipelineTypes = new List<Type>();

        // Act
        Action act = () => new MutationContext<object>(queryProvider, pipelineTypes);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("queryProvider", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenPipelineTypesIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var queryProvider = new Mock<ITransactionProvider<object>>().Object;
        List<Type> pipelineTypes = null!;
        
        // Act
        Action act = () => new MutationContext<object>(queryProvider, pipelineTypes);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("pipelineTypes", exception.ParamName);
    }

    [Fact]
    public void Constructor_InitializesEntities()
    {
        // Arrange
        var queryProvider = new Mock<ITransactionProvider<object>>().Object;
        var pipelineTypes = new List<Type>();

        // Act
        var context = new MutationContext<object>(queryProvider, pipelineTypes);
        
        // Assert
        Assert.NotNull(context.Entities);
        Assert.Empty(context.Entities);
    }
}