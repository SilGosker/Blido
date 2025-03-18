using System.Reflection;
using Blido.Core.Transaction.Mutation.KeyGeneration.KeyGenerators;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Blido.Core.Transaction.Mutation.KeyGeneration;

public class KeyGeneratorMiddlewareTests
{
    [Fact]
    public void Constructor_WithNullKeyGeneratorFactory_ThrowsArgumentNullException()
    {
        // Arrange
        IKeyGeneratorFactory keyGeneratorFactory = null!;
        
        // Act
        Action act = () => new KeyGeneratorMiddleware(keyGeneratorFactory);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("keyGeneratorFactory", exception.ParamName);
    }

    [Fact]
    public void ExecuteAsync_SetsPrimaryKeyOfAddedEntities()
    {
        // Arrange
        var keyGeneratorFactory = new KeyGeneratorFactory();
        var context = new MutationContext(new List<Type>(), new AsyncServiceScope());
        var updatedEntityId = Guid.NewGuid();
        var newEntity = new EntityWithGuidKey();
        var updateEntity = new EntityWithGuidKey
        {
            Key = updatedEntityId
        };
        context.Entities.Add(new MutationEntityContext(newEntity, MutationState.Added));
        context.Entities.Add(new MutationEntityContext(updateEntity, MutationState.Modified));
        var middleware = new KeyGeneratorMiddleware(keyGeneratorFactory);

        // Act
        middleware.ExecuteAsync(context, () => ValueTask.CompletedTask);

        // Assert
        Assert.NotNull(newEntity.Key);
        Assert.NotEqual(Guid.Empty, newEntity.Key);
        Assert.Equal(updatedEntityId, updateEntity.Key);
    }
}