namespace Blido.Core;

public class NameResolverTests
{
    [Fact]
    public void ResolveObjectStoreName_ShouldReturnName()
    {
        // Arrange
        var type = typeof(MockObjectStore);

        // Act
        var actual = NameResolver.ResolveObjectStoreName(type);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal("MockObjectStore", actual);
    }

    [Fact]
    public void ResolveObjectStoreName_WhenTypeHasObjectStoreNameAttribute_ShouldOverride()
    {
        // Arrange
        var type = typeof(MockObjectStoreWithAttribute);

        // Act
        var actual = NameResolver.ResolveObjectStoreName(type);
        
        // Assert
        Assert.Equal("CustomName", actual);
    }

    [Fact]
    public void ResolveObjectStoreName_WithGenericType_ShouldReturnName()
    {
        // Arrange & Act 
        var actual = NameResolver.ResolveObjectStoreName<MockObjectStore>();

        // Assert
        Assert.Equal("MockObjectStore", actual);
    }

    [Fact]
    public void ResolveObjectStoreName_WithGenericType_WhenTypeHasObjectStoreNameAttribute_ShouldOverride()
    {
        // Arrange & Act 
        var actual = NameResolver.ResolveObjectStoreName<MockObjectStoreWithAttribute>();

        // Assert
        Assert.Equal("CustomName", actual);
    }

    [Fact]
    public void ResolveObjectFieldName_ShouldReturnName()
    {
        // Arrange
        var property = typeof(MockObjectStore).GetProperty("Name")!;

        // Act
        var actual = NameResolver.ResolveObjectFieldName(property);
        
        // Assert
        Assert.Equal("Name", actual);
    }

    [Fact]
    public void ResolveObjectFieldName_WhenPropertyHasFieldNameAttribute_ShouldOverride()
    {
        // Arrange
        var property = typeof(MockObjectStoreWithFieldAttribute).GetProperty("Name")!;

        // Act
        var actual = NameResolver.ResolveObjectFieldName(property);

        // Assert
        Assert.Equal("__name", actual);
    }

    [Fact]
    public void ResolveIndexedDbName_ShouldReturnName()
    {
        // Arrange
        var type = typeof(MockIndexedDbDatabase);

        // Act
        var actual = NameResolver.ResolveIndexedDbName(type);

        // Assert
        Assert.Equal("MockIndexedDbDatabase", actual);
    }

    [Fact]
    public void ResolveIndexedDbName_WhenIndexedDbHasIndexedDbDatabaseNameAttribute_ShouldOverride()
    {
        // Arrange
        var type = typeof(MockIndexedDbDatabaseWithAttribute);

        // Act
        var actual = NameResolver.ResolveIndexedDbName(type);

        // Assert
        Assert.Equal("CustomName", actual);
    }

}