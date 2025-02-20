using System.Text;

namespace BlazorIndexedOrm.Core.Extensions;

public class StringBuilderExtensionsTests
{
    [Fact]
    public void AppendEscaped_WhenSbIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        StringBuilder sb = null!;
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
    [InlineData("Hello \'World'", "Hello \\\'World\\\'")] // String with double quotes
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
        sb.AppendEscaped(input);

        // Assert
        Assert.Equal(expected, sb.ToString());
    }

    [Fact]
    public void AppendEscapedWithInt_ShouldAppendValue()
    {
        // Arrange
        StringBuilder sb = new();
        object value = 42;

        // Act
        sb.AppendEscaped(value);

        // Assert
        Assert.Equal("42", sb.ToString());
    }

    [Fact]
    public void AppendEscapedWithDouble_ShouldAppendValue()
    {
        // Arrange
        StringBuilder sb = new();
        object value = (double)42.42;

        // Act
        sb.AppendEscaped(value);
        // Assert
        Assert.Equal("42.42", sb.ToString());
    }

    [Fact]
    public void AppendEscapedWithFloat_ShouldAppendValue()
    {
        // Arrange
        StringBuilder sb = new();
        object value = 42.42f;

        // Act
        sb.AppendEscaped(value);

        // Assert
        Assert.Equal("42.42", sb.ToString());
    }

    [Fact]
    public void AppendEscapedWithUInt_ShouldAppendValue()
    {
        // Arrange
        StringBuilder sb = new();
        object value = (uint)42;

        // Act
        sb.AppendEscaped(value);

        // Assert
        Assert.Equal("42", sb.ToString());
    }

    [Fact]
    public void AppendEscapedWithLong_ShouldAppendValue()
    {
        // Arrange
        StringBuilder sb = new();
        object value = 42L;

        // Act
        sb.AppendEscaped(value);

        // Assert
        Assert.Equal("42", sb.ToString());
    }

    [Fact]
    public void AppendEscapedWithULong_ShouldAppendValue()
    {
        // Arrange
        StringBuilder sb = new();
        object value = 42ul;

        // Act
        sb.AppendEscaped(value);

        // Assert
        Assert.Equal("42", sb.ToString());
    }

    [Fact]
    public void AppendEscapedWithShort_ShouldAppendValue()
    {
        // Arrange
        StringBuilder sb = new();
        object value = (short)42;

        // Act
        sb.AppendEscaped(value);

        // Assert
        Assert.Equal("42", sb.ToString());
    }

    [Fact]
    public void AppendEscapedWithUShort_ShouldAppendValue()
    {
        // Arrange
        StringBuilder sb = new();
        object value = (ushort)42;

        // Act
        sb.AppendEscaped(value);

        // Assert
        Assert.Equal("42", sb.ToString());
    }

    [Fact]
    public void AppendEscapedWithByte_ShouldAppendValue()
    {
        // Arrange
        StringBuilder sb = new();
        object value = (byte)42;

        // Act
        sb.AppendEscaped(value);

        // Assert
        Assert.Equal("42", sb.ToString());
    }

    [Fact]
    public void AppendEscapedWithSByte_ShouldAppendValue()
    {
        // Arrange
        StringBuilder sb = new();
        object value = (sbyte)42;

        // Act
        sb.AppendEscaped(value);

        // Assert
        Assert.Equal("42", sb.ToString());
    }

    [Theory]
    [InlineData('a', "'a'")]
    [InlineData('\'', "'\\\''")]
    public void AppendEscapedWithChar_ShouldAppendValue(char input, string expected)
    {
        // Arrange
        StringBuilder sb = new();
        object value = input;

        // Act
        sb.AppendEscaped(value);

        // Assert
        Assert.Equal(expected, sb.ToString());
    }

    [Theory]
    [InlineData("test", "\"test\"")]
    [InlineData("test\"test", "\"test\\\"test\"")]
    public void AppendEscapedWithString_ShouldAppendValue(string input, string expected)
    {
        // Arrange
        StringBuilder sb = new();
        object value = input;

        // Act
        sb.AppendEscaped(value);

        // Assert
        Assert.Equal(expected, sb.ToString());
    }

    [Theory]
    [InlineData("en-US", "\"en\"")]
    [InlineData("de-DE", "\"de\"")]
    public void AppendEscapedWithCultureInfo_ShouldAppendValue(string input, string expected)
    {
        // Arrange
        StringBuilder sb = new();
        object value = new System.Globalization.CultureInfo(input);

        // Act
        sb.AppendEscaped(value);

        // Assert
        Assert.Equal(expected, sb.ToString());
    }

    [Fact]
    public void AppendEscapedWithBoolTrue_ShouldAppendValue()
    {
        // Arrange
        StringBuilder sb = new();
        object value = true;

        // Act
        sb.AppendEscaped(value);

        // Assert
        Assert.Equal("true", sb.ToString());
    }

    [Fact]
    public void AppendEscapedWithBoolFalse_ShouldAppendValue()
    {
        // Arrange
        StringBuilder sb = new();
        object value = false;

        // Act
        sb.AppendEscaped(value);

        // Assert
        Assert.Equal("false", sb.ToString());
    }

    [Fact]
    public void AppendEscapedWithNull_ShouldAppendValue()
    {
        // Arrange
        StringBuilder sb = new();
        object? value = null;

        // Act
        sb.AppendEscaped(value);

        // Assert
        Assert.Equal("null", sb.ToString());
    }

    [Fact]
    public void AppendEscapedWithIEnumerable_ShouldAppendValue()
    {
        // Arrange
        StringBuilder sb = new();
        object value = new[] { 1, 2, 3 };

        // Act
        sb.AppendEscaped(value);

        // Assert
        Assert.Equal("[1,2,3]", sb.ToString());
    }

    [Fact]
    public void AppendEscapedWithIEnumerableOfIEnumerable_ShouldAppendValue()
    {
        // Arrange
        StringBuilder sb = new();
        object value = new[] { new[] { 1, 2 }, new[] { 3, 4 } };

        // Act
        sb.AppendEscaped(value);

        // Assert
        Assert.Equal("[[1,2],[3,4]]", sb.ToString());
    }

    [Fact]
    public void AppendEscapedWithIndex_ShouldAppendValue()
    {
        // Arrange
        StringBuilder sb = new();
        object value = new Index(42);

        // Act
        sb.AppendEscaped(value);

        // Assert
        Assert.Equal("42", sb.ToString());
    }
}