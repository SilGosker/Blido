using Blido.Core.Options;
using Blido.Core.Transaction.Mutation;
using Blido.Core.Transaction.Mutation.KeyGeneration;

namespace Blido.Core.Extensions;

public static class MutationConfigurationExtensions
{
    public static MutationConfiguration UseKeyInitialization(this MutationConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        configuration.Use<KeyGeneratorMiddleware>();
        return configuration;
    }

    public static MutationConfiguration Mutate(this MutationConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        configuration.Use<MutateMiddleware>();
        return configuration;
    }
}