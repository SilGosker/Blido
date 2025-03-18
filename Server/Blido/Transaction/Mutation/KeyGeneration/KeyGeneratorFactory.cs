using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Blido.Core.Helpers;

namespace Blido.Core.Transaction.Mutation.KeyGeneration;

public class KeyGeneratorFactory : IKeyGeneratorFactory
{
    private IReadOnlyDictionary<Type, GenerateKeyDelegate> _keyGenerators = new Dictionary<Type, GenerateKeyDelegate>();

    public bool TryGetValue(PropertyInfo key, [MaybeNullWhen(false)] out GenerateKeyDelegate value)
    {
        ArgumentNullException.ThrowIfNull(key);
        return _keyGenerators.TryGetValue(key.PropertyType, out value);
    }

    void IKeyGeneratorFactory.Confirm()
    {
        _keyGenerators = _keyGenerators.ToFrozenDictionary();
    }

    public void AddOrReplace<TKeyGenerator>() where TKeyGenerator : IKeyGenerator
    {
        foreach (var supportedType in TKeyGenerator.SupportedTypes)
        {
            AddOrReplace(supportedType, TKeyGenerator.ApplyKey);
        }
    }

    public void AddOrReplace(Type info, GenerateKeyDelegate generator)
    {
        ArgumentNullException.ThrowIfNull(info);
        ArgumentNullException.ThrowIfNull(generator);
        ThrowHelper.ThrowDictionaryIsNotReadonlyException(_keyGenerators, out var dictionary);
        dictionary[info] = generator;
    }
}