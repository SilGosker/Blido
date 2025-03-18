namespace Blido.Core.Transaction.Mutation.KeyGeneration.KeyGenerators;

public class GuidKeyGeneratorTests
{
    [Fact]
    public void SupportedTypes_DoesNotContainNull()
    {
        // Arrange
        var supportedTypes = GuidKeyGenerator.SupportedTypes;

        // Act
        var containsNull = supportedTypes.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void ApplyKey_WhenKeyHasDefaultValue_GeneratesNewKey()
    {
        // Arrange
        var entity = new EntityWithGuidKey { Key = Guid.Empty };
        var propertyInfo = entity.GetType().GetProperty(nameof(EntityWithGuidKey.Key))!;

        // Act
        GuidKeyGenerator.ApplyKey(entity, propertyInfo);
        
        // Assert
        Assert.NotEqual(Guid.Empty, entity.Key);
    }

    [Fact]
    public void ApplyKey_WhenKeyIsNull_GeneratesNewKey()
    {
        // Arrange
        var entity = new EntityWithGuidKey { Key = null };
        var propertyInfo = entity.GetType().GetProperty(nameof(entity.Key))!;
        // Act
        GuidKeyGenerator.ApplyKey(entity, propertyInfo);

        // Assert
        Assert.NotNull(entity.Key);
    }

    [Fact]
    public void ApplyKey_WhenKeyHasValue_DoesNotGenerateNewKey()
    {
        // Arrange
        var key = Guid.NewGuid();
        var entity = new EntityWithGuidKey { Key = key };
        var propertyInfo = entity.GetType().GetProperty(nameof(entity.Key))!;

        // Act
        GuidKeyGenerator.ApplyKey(entity, propertyInfo);

        // Assert
        Assert.Equal(key, entity.Key);
    }
}