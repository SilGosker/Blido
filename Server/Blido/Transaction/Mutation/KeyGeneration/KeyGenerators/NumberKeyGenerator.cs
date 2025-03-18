using Blido.Core.Helpers;

namespace Blido.Core.Transaction.Mutation.KeyGeneration.KeyGenerators;

public class NumberKeyGenerator : IKeyGenerator
{
    public static GenerateKeyDelegate ApplyKey => (entity, propertyInfo) =>
    {
        var value = propertyInfo.GetValue(entity);
        if (value is null || (int)value == 0)
        {
            propertyInfo.SetValue(entity, 0);
        }
    };

    public static Type[] SupportedTypes => NumberHelper.NumberTypes
        .Concat(NumberHelper.NumberTypes
            .Select(x => typeof(Nullable<>).MakeGenericType(x))).ToArray();
}