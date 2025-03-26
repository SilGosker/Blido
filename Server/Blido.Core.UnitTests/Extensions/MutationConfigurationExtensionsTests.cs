using Blido.Core.Options;
using Blido.Core.Transaction.Mutation;
using Blido.Core.Transaction.Mutation.KeyGeneration;

namespace Blido.Core.Extensions;

public class MutationConfigurationExtensionsTests
{
    [Fact]
    public void UseInitialization_WhenConfigurationIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        MutationConfiguration configuration = null!;

        // Act
        Action act = () => configuration.UseKeyInitialization();
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("configuration", exception.ParamName);
    }

    [Fact]
    public void UseInitialization_WhenConfigurationIsNotNull_ShouldAddToMiddlewareTypes()
    {
        // Arrange
        var configuration = new MutationConfiguration();

        // Act
        var result = configuration.UseKeyInitialization();

        // Assert
        Assert.NotNull(result);
        Assert.Contains(configuration.MiddlewareTypes, x => x.Type == typeof(KeyGeneratorMiddleware));
    }

    [Fact]
    public void UseInitialization_ReturnsInstance()
    {
        // Arrange
        var configuration = new MutationConfiguration();
        
        // Act
        var result = configuration.UseKeyInitialization();

        // Assert
        Assert.NotNull(result);
        Assert.Same(configuration, result);
    }

    [Fact]
    public void Mutate_WhenConfigurationIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        MutationConfiguration configuration = null!;

        // Act
        Action act = () => configuration.Mutate();

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("configuration", exception.ParamName);
    }

    [Fact]
    public void Mutate_WhenConfigurationIsNotNull_ShouldAddToMiddlewareTypes()
    {
        // Arrange
        var configuration = new MutationConfiguration();

        // Act
        var result = configuration.Mutate();

        // Assert
        Assert.NotNull(result);
        Assert.Contains(configuration.MiddlewareTypes, x => x.Type == typeof(MutateMiddleware));
    }

    [Fact]
    public void Mutate_ReturnsInstance()
    {
        // Arrange
        var configuration = new MutationConfiguration();
        
        // Act
        var result = configuration.Mutate();

        // Assert
        Assert.NotNull(result);
        Assert.Same(configuration, result);
    }
}