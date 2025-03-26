using Blido.Core.Options;
using Blido.Core.Transaction.JsExpression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Moq;

namespace Blido.Core.Transaction.Mutation.Arguments;

public class MutateArgumentTests
{
    [Fact]
    public async Task CreateAsync_WhenMutationContextIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        MutationContext mutationContext = null!;
        var cancellationToken = CancellationToken.None;

        // Act
        async Task Act() => await MutateArguments.CreateAsync(mutationContext, cancellationToken);
        
        // Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(Act);
        Assert.Equal("mutationContext", exception.ParamName);
    }

    [Fact]
    public void CreateAsync_WithoutEntities_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var jsRuntimeMock = new Mock<IJSRuntime>();
        var mockObjectStoreFactory = new Mock<IObjectStoreFactory>();
        mockObjectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntimeMock.Object);
        var context = new MockIndexedDbDatabase(mockObjectStoreFactory.Object);

        var mutationContext = new MutationContext(new OptionsWrapper<MutationConfiguration>(new MutationConfiguration()), new Mock<IServiceProvider>().Object, context);
        var cancellationToken = CancellationToken.None;

        // Act
        async Task Act() => await MutateArguments.CreateAsync(mutationContext, cancellationToken);
        
        // Assert
        var exception = Assert.ThrowsAsync<ArgumentOutOfRangeException>(Act);
    }
}