using Blido.Core.Extensions;

namespace Blido.Core.Options;

public class BlidoConfigurationTests
{
    [Fact]
    public void ConfigureMutationPipeline_WhenConfigureIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var configuration = new BlidoConfiguration();
        Action<MutationConfiguration> configure = null!;

        // Act
        Action action = () => configuration.ConfigureMutationPipeline(configure);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("configure", exception.ParamName);
    }

    [Fact]
    public void ConfigureMutationPipeline_WhenConfigureIsNotNull_ShouldSetMutationPipelineConfiguration()
    {
        // Arrange
        var configuration = new BlidoConfiguration();
        Action<MutationConfiguration> configure = app => app.UseKeyInitialization();

        // Act
        configuration.ConfigureMutationPipeline(configure);

        // Assert
        Assert.NotNull(configuration.MutationPipelineConfiguration);
        Assert.Same(configure, configuration.MutationPipelineConfiguration);
    }
}