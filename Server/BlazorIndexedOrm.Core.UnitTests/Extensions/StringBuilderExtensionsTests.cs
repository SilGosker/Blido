using System.Text;

namespace BlazorIndexedOrm.Core.Extensions;

public class StringBuilderExtensionsTests
{
    [Fact]
    public void AppendEscaped_WhenSbIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        StringBuilder sb = null;
        string s = "test";

        // Act
        Action act = () => sb!.AppendEscaped(s);

        // Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("sb", exception.ParamName);
    }

    [Theory]
    [InlineData("Hello", "Hello")] // Simple string
    [InlineData("Hello \"World\"", "Hello \\\"World\\\"")] // String with double quotes
    [InlineData("Path\\To\\File", "Path\\\\To\\\\File")] // String with backslashes
    [InlineData("Line1\nLine2", "Line1\\nLine2")] // String with newlines
    [InlineData("\tTabbed", "\\tTabbed")] // String with tabs
    [InlineData("Carriage\rReturn", "Carriage\\rReturn")] // String with carriage return
    [InlineData("Emoji 😊", "Emoji 😊")] // String with Unicode characters (e.g., emojis)
    [InlineData("", "")] // Empty string
    [InlineData(null, "")] // Empty string
    public void AppendEscaped_ValidInput_ProducesCorrectOutput(string input, string expected)
    {
        // Arrange
        var sb = new StringBuilder();

        // Act
        sb.AppendEscaped(input.AsSpan());

        // Assert
        Assert.Equal(expected, sb.ToString());
    }
}