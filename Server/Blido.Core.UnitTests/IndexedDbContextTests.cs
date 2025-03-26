using Blido.Core.Options;
using Blido.Core.Transaction;
using Blido.Core.Transaction.JsExpression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Moq;

namespace Blido.Core;

public class IndexedDbContextTests
{
    [Fact]
    public void Constructor_WhenPassedNull_ThrowsArgumentNullException()
    {
        // Arrange
        IObjectStoreFactory mockTransactionProviderFactory = null!;

        // Act
        var act = () => new MockIndexedDbDatabase(mockTransactionProviderFactory);

        // Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("transactionProviderFactory", exception.ParamName);
    }


    [Fact]
    public void Constructor_InitializesDatabase()
    {
        // Arrange
        var mockTransactionProviderFactory = new Mock<IObjectStoreFactory>();
        var mockJsRuntime = new Mock<IJSRuntime>();
        mockTransactionProviderFactory.SetupGet(x => x.JsRuntime).Returns(mockJsRuntime.Object);

        // Act
        var mockIndexedDbDatabase = new MockIndexedDbDatabase(mockTransactionProviderFactory.Object);
        
        // Assert
        Assert.NotNull(mockIndexedDbDatabase.Database);
    }

    [Fact]
    public void Constructor_InitializesProperties()
    {
        // Arrange
        var mockTransactionProviderFactory = new Mock<IObjectStoreFactory>();
        mockTransactionProviderFactory.Setup(x => x.GetObjectStore(It.IsAny<IndexedDbContext>(), It.IsAny<Type>()))
            .Returns((IndexedDbContext database, Type type) =>
            {
                if (type == typeof(object))
                {
                    return new ObjectStore<object>(database, new Mock<ITransactionProvider<object>>().Object, new Mock<IServiceProvider>().Object, new OptionsWrapper<MutationConfiguration>(new MutationConfiguration()));
                }
                return new ObjectStore<string>(database, new Mock<ITransactionProvider<string>>().Object, new Mock<IServiceProvider>().Object, new OptionsWrapper<MutationConfiguration>(new MutationConfiguration()));
            });
        var mockJsRuntime = new Mock<IJSRuntime>();
        mockTransactionProviderFactory.SetupGet(x => x.JsRuntime).Returns(mockJsRuntime.Object);

        // Act
        var mockIndexedDbDatabase = new MockIndexedDbDatabaseWithObjectStoreSetProperties(mockTransactionProviderFactory.Object);
        
        // Assert
        mockTransactionProviderFactory.Verify(x => x.GetObjectStore(It.IsAny<IndexedDbContext>(), It.IsAny<Type>()), Times.Exactly(2));
        Assert.NotNull(mockIndexedDbDatabase.Objects);
        Assert.NotNull(mockIndexedDbDatabase.Strings);
    }

}