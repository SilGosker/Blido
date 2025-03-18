namespace Blido.Core.Transaction.Mutation.KeyGeneration.KeyGenerators;

public class GuidKeyGenerator : IKeyGenerator
{
    public static GenerateKeyDelegate ApplyKey => (entity, propertyInfo) =>
    {
        var value = propertyInfo.GetValue(entity);
        if (value is null || (Guid)value == Guid.Empty)
        {
            propertyInfo.SetValue(entity, Guid.NewGuid());
        }
    };

    public static Type[] SupportedTypes => new[]
    {
        typeof(Guid),
        typeof(Guid?)
    };
}