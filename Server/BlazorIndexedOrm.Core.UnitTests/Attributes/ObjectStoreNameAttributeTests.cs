namespace BlazorIndexedOrm.Core.Attributes;

public class ObjectStoreNameAttributeTests
{
    [Fact]
    public void Constructor_WithNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        string name = null!;

        // Act
        var act = () => new ObjectStoreNameAttribute(name);

        // Assert
        Assert.Throws<ArgumentNullException>(act);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void Constructor_WithEmptyString_ShouldThrowArgumentException(string name)
    {
        // Act
        var act = () => new ObjectStoreNameAttribute(name);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Constructor_WithValidName_ShouldSetName()
    {
        // Arrange
        var name = "name";
        
        // Act
        var attribute = new ObjectStoreNameAttribute(name);
     
        // Assert
        Assert.Equal(name, attribute.Name);
    }
}