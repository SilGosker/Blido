using System.Reflection;
using Blido.Core.Transaction.JsExpression;
using Blido.Core.Transaction.Mutation.KeyGeneration.KeyGenerators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
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
        var jsRuntime = new Mock<IJSRuntime>();
        var context = new MutationContext(new List<Type>(), new AsyncServiceScope(), new MockIndexedDbDatabase(new ObjectStoreFactory(new Mock<IExpressionBuilder>().Object, jsRuntime.Object)));
        var updatedEntityId = Guid.NewGuid();
        var newEntity = new EntityWithGuidKey();
        var updateEntity = new EntityWithGuidKey
        {
            Key = updatedEntityId
        };
        context.Insert(newEntity);
        context.Update(updateEntity);
        var middleware = new KeyGeneratorMiddleware(keyGeneratorFactory);

        // Act
        middleware.ExecuteAsync(context, () => ValueTask.CompletedTask, CancellationToken.None);

        // Assert
        Assert.NotNull(newEntity.Key);
        Assert.NotEqual(Guid.Empty, newEntity.Key);
        Assert.Equal(updatedEntityId, updateEntity.Key);
    }
}