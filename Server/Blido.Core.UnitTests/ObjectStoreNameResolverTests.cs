namespace Blido.Core;

public class NameResolverTests
{
    [Fact]
    public void ResolveObjectStoreName_ShouldReturnName()
    {
        // Arrange & Act 
        var actual = NameResolver.ResolveObjectStoreName<MockObjectStore>();

        // Assert
        Assert.Equal("MockObjectStore", actual);
    }

    [Fact]
    public void ResolveObjectStoreName_WhenTypeHasObjectStoreNameAttribute_ShouldOverride()
    {
        // Arrange & Act 
        var actual = NameResolver.ResolveObjectStoreName<MockObjectStoreWithAttribute>();

        // Assert
        Assert.Equal("CustomName", actual);
    }

}