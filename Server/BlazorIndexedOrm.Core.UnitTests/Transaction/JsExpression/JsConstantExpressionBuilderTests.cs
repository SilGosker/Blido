using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression;

public class JsConstantExpressionBuilderTests
{
    [Fact]
    public void Constructor_ShouldNotThrow()
    {
        // Arrange & Act
        JsConstantExpressionBuilder? builder = new JsConstantExpressionBuilder();

        // Assert
        Assert.NotNull(builder);
    }

    [Fact]
    public void AppendConstant_WithInt_ShouldAppendValue()
    {
        // Arrange
        StringBuilder sb = new();
        ConstantExpression expression = Expression.Constant(42);

        // Act
        JsConstantExpressionBuilder.AppendConstant(sb, expression);

        // Assert
        Assert.Equal("42", sb.ToString());
    }

    [Fact]
    public void AppendConstant_WithDouble_ShouldAppendValue()
    {
        // Arrange
        StringBuilder sb = new();
        ConstantExpression expression = Expression.Constant(42.42);

        // Act
        JsConstantExpressionBuilder.AppendConstant(sb, expression);
        // Assert
        Assert.Equal("42.42", sb.ToString());
    }

    [Fact]
    public void AppendConstant_WithFloat_ShouldAppendValue()
    {
        // Arrange
        StringBuilder sb = new();
        ConstantExpression expression = Expression.Constant(42.42f);

        // Act
        JsConstantExpressionBuilder.AppendConstant(sb, expression);

        // Assert
        Assert.Equal("42.42", sb.ToString());
    }

    [Fact]
    public void AppendConstant_WithUInt_ShouldAppendValue()
    {
        // Arrange
        StringBuilder sb = new();
        ConstantExpression expression = Expression.Constant((uint)42);

        // Act
        JsConstantExpressionBuilder.AppendConstant(sb, expression);

        // Assert
        Assert.Equal("42", sb.ToString());
    }

    [Fact]
    public void AppendConstant_WithLong_ShouldAppendValue()
    {
        // Arrange
        StringBuilder sb = new();
        ConstantExpression expression = Expression.Constant(42L);

        // Act
        JsConstantExpressionBuilder.AppendConstant(sb, expression);

        // Assert
        Assert.Equal("42", sb.ToString());
    }

    [Fact]
    public void AppendConstant_WithULong_ShouldAppendValue()
    {
        // Arrange
        StringBuilder sb = new();
        ConstantExpression expression = Expression.Constant(42UL);

        // Act
        JsConstantExpressionBuilder.AppendConstant(sb, expression);

        // Assert
        Assert.Equal("42", sb.ToString());
    }

    [Fact]
    public void AppendConstant_WithShort_ShouldAppendValue()
    {
        // Arrange
        StringBuilder sb = new();
        ConstantExpression expression = Expression.Constant((short)42);

        // Act
        JsConstantExpressionBuilder.AppendConstant(sb, expression);

        // Assert
        Assert.Equal("42", sb.ToString());
    }

    [Fact]
    public void AppendConstant_WithUShort_ShouldAppendValue()
    {
        // Arrange
        JsConstantExpressionBuilder builder = new ();
        StringBuilder sb = new();
        ConstantExpression expression = Expression.Constant((ushort)42);

        // Act
        JsConstantExpressionBuilder.AppendConstant(sb, expression);

        // Assert
        Assert.Equal("42", sb.ToString());
    }

    [Fact]
    public void AppendConstant_WithByte_ShouldAppendValue()
    {
        // Arrange
        JsConstantExpressionBuilder builder = new ();
        StringBuilder sb = new();
        ConstantExpression expression = Expression.Constant((byte)42);

        // Act
        JsConstantExpressionBuilder.AppendConstant(sb, expression);

        // Assert
        Assert.Equal("42", sb.ToString());
    }

    [Fact]
    public void AppendConstant_WithSByte_ShouldAppendValue()
    {
        // Arrange
        JsConstantExpressionBuilder builder = new ();
        StringBuilder sb = new();
        ConstantExpression expression = Expression.Constant((sbyte)42);

        // Act
        JsConstantExpressionBuilder.AppendConstant(sb, expression);

        // Assert
        Assert.Equal("42", sb.ToString());
    }

    [Theory]
    [InlineData('a', "'a'")]
    [InlineData('\'', "'\\\''")]
    public void AppendConstant_WithChar_ShouldAppendValue(char input, string expected)
    {
        // Arrange
        StringBuilder sb = new();
        ConstantExpression expression = Expression.Constant(input);

        // Act
        JsConstantExpressionBuilder.AppendConstant(sb, expression);

        // Assert
        Assert.Equal(expected, sb.ToString());
    }

    [Theory]
    [InlineData("test", "\"test\"")]
    [InlineData("test\"test", "\"test\\\"test\"")]
    public void AppendConstant_WithString_ShouldAppendValue(string input, string expected)
    {
        // Arrange
        StringBuilder sb = new();
        ConstantExpression expression = Expression.Constant(input);

        // Act
        JsConstantExpressionBuilder.AppendConstant(sb, expression);
        
        // Assert
        Assert.Equal(expected, sb.ToString());
    }

    [Theory]
    [InlineData("en-US", "\"en\"")]
    [InlineData("de-DE", "\"de\"")]
    public void AppendConstant_WithCultureInfo_ShouldAppendValue(string input, string expected)
    {
        // Arrange
        StringBuilder sb = new();
        ConstantExpression expression = Expression.Constant(new System.Globalization.CultureInfo(input));

        // Act
        JsConstantExpressionBuilder.AppendConstant(sb, expression);

        // Assert
        Assert.Equal(expected, sb.ToString());
    }

    [Fact]
    public void AppendConstant_WithBoolTrue_ShouldAppendValue()
    {
        // Arrange
        StringBuilder sb = new();
        ConstantExpression expression = Expression.Constant(true);

        // Act
        JsConstantExpressionBuilder.AppendConstant(sb, expression);

        // Assert
        Assert.Equal("true", sb.ToString());
    }

    [Fact]
    public void AppendConstant_WithBoolFalse_ShouldAppendValue()
    {
        // Arrange
        JsConstantExpressionBuilder builder = new ();
        StringBuilder sb = new();
        ConstantExpression expression = Expression.Constant(false);

        // Act
        JsConstantExpressionBuilder.AppendConstant(sb, expression);

        // Assert
        Assert.Equal("false", sb.ToString());
    }

    [Fact]
    public void AppendConstant_WithNull_ShouldAppendValue()
    {
        // Arrange
        JsConstantExpressionBuilder builder = new ();
        StringBuilder sb = new();
        ConstantExpression expression = Expression.Constant(null);

        // Act
        JsConstantExpressionBuilder.AppendConstant(sb, expression);

        // Assert
        Assert.Equal("null", sb.ToString());
    }

    [Fact]
    public void AppendConstant_WithIEnumerable_ShouldAppendValue()
    {
        // Arrange
        JsConstantExpressionBuilder builder = new ();
        StringBuilder sb = new();
        ConstantExpression expression = Expression.Constant(new[] { 1, 2, 3 });

        // Act
        JsConstantExpressionBuilder.AppendConstant(sb, expression);

        // Assert
        Assert.Equal("[1,2,3]", sb.ToString());
    }

    [Fact]
    public void AppendConstant_WithIEnumerableOfIEnumerable_ShouldAppendValue()
    {
        // Arrange
        JsConstantExpressionBuilder builder = new ();
        StringBuilder sb = new();
        ConstantExpression expression = Expression.Constant(new[] { new[] { 1, 2 }, new[] { 3, 4 } });

        // Act
        JsConstantExpressionBuilder.AppendConstant(sb, expression);

        // Assert
        Assert.Equal("[[1,2],[3,4]]", sb.ToString());
    }

    [Fact]
    public void AppendConstant_WithIndex_ShouldAppendValue()
    {
        // Arrange
        StringBuilder sb = new();
        ConstantExpression expression = Expression.Constant(new Index(42));

        // Act
        JsConstantExpressionBuilder.AppendConstant(sb, expression);

        // Assert
        Assert.Equal("42", sb.ToString());
    }
}