namespace Blido.Core.Transaction.Mutation.KeyGeneration.KeyGenerators;

public class NumberKeyGeneratorTests
{
    [Fact]
    public void SupportedTypes_DoesNotContainNull()
    {
        // Arrange
        var supportedTypes = NumberKeyGenerator.SupportedTypes;

        // Act
        var containsNull = supportedTypes.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void ApplyKey_WhenKeyIsNull_SetsKeyToZero()
    {
        // Arrange
        var entity = new EntityWithNumberKey { Key = null };
        var propertyInfo = entity.GetType().GetProperty(nameof(entity.Key))!;

        // Act
        NumberKeyGenerator.ApplyKey(entity, propertyInfo);

        // Assert
        Assert.Equal(0, entity.Key);
    }

    [Fact]
    public void ApplyKey_WhenKeyHasValue_DoesNotGenerateNewKey()
    {
        // Arrange
        var key = 1;
        var entity = new EntityWithNumberKey { Key = key };
        var propertyInfo = entity.GetType().GetProperty(nameof(entity.Key))!;
        // Act
        NumberKeyGenerator.ApplyKey(entity, propertyInfo);
        // Assert
        Assert.Equal(key, entity.Key);
    }
}