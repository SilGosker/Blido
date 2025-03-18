namespace Blido.Core.Transaction.Mutation.KeyGeneration;

public interface IKeyGenerator
{
    public static abstract GenerateKeyDelegate ApplyKey { get; }
    public static abstract Type[] SupportedTypes { get; }
}