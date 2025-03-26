using Blido.Core.Extensions;

namespace Blido.Core.Options;

public class BlidoConfiguration
{
    internal Action<MutationConfiguration> MutationPipelineConfiguration { get; set; } = app =>
    {
        app.UseKeyInitialization();

        app.Mutate();
    };

    public void ConfigureMutationPipeline(Action<MutationConfiguration> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        MutationPipelineConfiguration = configure;
    }
}