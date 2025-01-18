using BlazorIndexedOrm.Core.ObjectStore;
using BlazorIndexedOrm.Core.UnitTests.Mock.ObjectStore.ObjectStore;

namespace BlazorIndexedOrm.Core.UnitTests.ObjectStore;

public class ObjectStoreNameResolverTests
{
    [Fact]
    public void Resolve_ShouldReturnName()
    {
        // Arrange & Act 
        var actual = ObjectStoreNameResolver.Resolve<MockObjectStore>();

        // Assert
        Assert.Equal("MockObjectStore", actual);
    }

    [Fact]
    public void Resolve_WhenTypeHasObjectStoreNameAttribute_ShouldOverride()
    {
        // Arrange & Act 
        var actual = ObjectStoreNameResolver.Resolve<MockObjectStoreWithAttribute>();

        // Assert
        Assert.Equal("CustomName", actual);
    }

}