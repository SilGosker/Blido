namespace Blido.Core.Attributes;

public class FieldNameAttributeTests
{
    [Fact]
    public void Constructor_WhenNameIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        string name = null!;

        // Act
        var act = () => new FieldNameAttribute(name);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("name", exception.ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void Constructor_WhenNameEmptyOrWhiteSpace_ShouldThrowArgumentException(string? name)
    {
        // Arrange
        // Act

        Action act = () => new FieldNameAttribute(name!);
        
        // Assert
        var exception = Assert.Throws<ArgumentException>(act);
        Assert.Equal("name", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenNameIsValid_ShouldSetNameProperty()
    {
        // Arrange
        string name = "Test";

        // Act
        var attribute = new FieldNameAttribute(name);

        // Assert
        Assert.Equal(name, attribute.Name);
    }
}