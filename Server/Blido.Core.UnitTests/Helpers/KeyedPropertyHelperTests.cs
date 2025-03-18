namespace Blido.Core.Helpers;

public class KeyedPropertyHelperTests
{
    [Fact]
    public void GetKeys_WhenTypeIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        Type type = null!;

        // Act
        Action act = () => KeyedPropertyHelper.GetKeys(type);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("type", exception.ParamName);
    }
    [Fact]
    public void GetKeys_WhenEntityHasNoValidKeys_ReturnsEmpty()
    {
        // Arrange
        var type = typeof(object);

        // Act
        var keys = KeyedPropertyHelper.GetKeys(type);

        // Assert
        Assert.Empty(keys);
    }

    [Fact]
    public void GetKeys_WhenEntityHasIdProperty_ReturnsProperty()
    {
        // Arrange
        var type = typeof(EntityWithId);

        // Act
        var keys = KeyedPropertyHelper.GetKeys(type).ToArray();

        // Assert
        Assert.Single(keys);
        Assert.Equal("Id", keys.First().Name);
    }

    [Fact]
    public void GetKeys_WhenEntityHasKeyAttribute_ReturnsProperty()
    {
        // Arrange
        var type = typeof(EntityWithKey);

        // Act
        var keys = KeyedPropertyHelper.GetKeys(type).ToArray();
        
        // Assert
        Assert.Single(keys);
        Assert.Equal("Key", keys.First().Name);
    }

    [Fact]
    public void GetKeys_WhenEntityHasKeysAttribute_ReturnsProperties()
    {
        // Arrange
        var type = typeof(EntityWithKeys);

        // Act
        var keys = KeyedPropertyHelper.GetKeys(type).ToArray();
        
        // Assert
        Assert.Equal(2, keys.Length);
        Assert.Contains(keys, x => x.Name == "Key1");
        Assert.Contains(keys, x => x.Name == "Key2");
    }

    [Fact]
    public void GetKeys_WhenEntityHasIdAndKeyAttribute_ReturnsKeyProperties()
    {
        // Arrange
        var type = typeof(EntityWithIdAndKey);

        // Act
        var keys = KeyedPropertyHelper.GetKeys(type).ToArray();

        // Assert
        Assert.Single(keys);
        Assert.Contains(keys, x => x.Name == "Key");
    }

    [Fact]
    public void GetKeys_WhenEntityHasMultipleIdProperties_ReturnsAllProperties()
    {
        // Arrange
        var type = typeof(EntityWithMultipleIds);

        // Act
        var keys = KeyedPropertyHelper.GetKeys(type).ToArray();
        
        // Assert
        Assert.Equal(4, keys.Length);
        Assert.Contains(keys, x => x.Name == "Id");
        Assert.Contains(keys, x => x.Name == "ID");
        Assert.Contains(keys, x => x.Name == "id");
        Assert.Contains(keys, x => x.Name == "iD");
    }

}