using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Blido.Core.Transaction.Mutation.KeyGeneration;

public interface IKeyGeneratorFactory
{
    internal void Confirm();
    public bool TryGetValue(PropertyInfo propertyInfo, [MaybeNullWhen(false)] out GenerateKeyDelegate generateKeyDelegate);
    public void AddOrReplace(Type info, GenerateKeyDelegate generator);
    public void AddOrReplace<TKeyGenerator>() where TKeyGenerator : IKeyGenerator;
}