using System.Reflection;
using Blido.Core.Options;
using Blido.Core.Transaction.JsExpression;
using Blido.Core.Transaction.Mutation.KeyGeneration.KeyGenerators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
        var serviceProvider = new Mock<IServiceProvider>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        objectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntime.Object);
        var dbContext = new MockIndexedDbDatabaseWithPrimaryKeySetProperties(objectStoreFactory.Object);
        var context = new MutationContext(new OptionsWrapper<MutationConfiguration>(new MutationConfiguration()), serviceProvider, dbContext);

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