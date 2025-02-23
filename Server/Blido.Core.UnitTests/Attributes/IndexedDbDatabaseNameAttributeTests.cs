namespace Blido.Core.Attributes;

public class IndexedDbDatabaseNameAttributeTests
{
    [Fact]
    public void Constructor_WithNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        string name = null!;

        // Act
        var act = () => new IndexedDbDatabaseNameAttribute(name);

        // Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("name", exception.ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void Constructor_WithEmptyString_ShouldThrowArgumentException(string name)
    {
        // Act
        var act = () => new IndexedDbDatabaseNameAttribute(name);

        // Assert
        ArgumentException exception = Assert.Throws<ArgumentException>(act);
        Assert.Equal("name", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithName_ShouldSetNameProperty()
    {
        // Arrange
        string name = "CustomName";

        // Act
        var attribute = new IndexedDbDatabaseNameAttribute(name);

        // Assert
        Assert.Equal(name, attribute.Name);
    }
}