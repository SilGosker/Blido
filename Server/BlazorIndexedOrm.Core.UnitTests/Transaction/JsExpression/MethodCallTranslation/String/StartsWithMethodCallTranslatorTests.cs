using System.Linq.Expressions;
using System.Text;
using Moq;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class StartsWithMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = StartsWithMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_WithoutStringComparison_ShouldAppendStartsWith()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, new[] { Expression.Constant("start") });
        var builder = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression { Value: string or char } constantExpression)
            {
                builder.Append('\"');
                builder.Append(constantExpression.Value);
                builder.Append('\"');
            }
        };
        
        // Act
        StartsWithMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);

        // Assert
        Assert.Equal("\"base\".startsWith(\"start\")", builder.ToString());
    }

    [Theory]
    [InlineData(StringComparison.CurrentCulture)]
    [InlineData(StringComparison.InvariantCulture)]
    [InlineData(StringComparison.Ordinal)]
    public void TranslateMethodCall_WithStringComparisonArgument_ShouldAppendStartsWith(StringComparison comparison)
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string), typeof(StringComparison) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, new Expression[] { Expression.Constant("start"), Expression.Constant(comparison) });
        var builder = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression { Value: string or char } constantExpression)
            {
                builder.Append('\"');
                builder.Append(constantExpression.Value);
                builder.Append('\"');
            }
        };

        // Act
        StartsWithMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);

        // Assert
        Assert.Equal("\"base\".startsWith(\"start\")", builder.ToString());
    }

    [Theory]
    [InlineData(StringComparison.CurrentCultureIgnoreCase)]
    [InlineData(StringComparison.InvariantCultureIgnoreCase)]
    [InlineData(StringComparison.OrdinalIgnoreCase)]
    public void TranslateMethodCall_WithIgnoreCaseStringComparison_ShouldAppendToUpperCaseAndStartsWith(StringComparison comparison)
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string), typeof(StringComparison) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, new Expression[] { Expression.Constant("start"), Expression.Constant(comparison) });
        var builder = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression { Value: string or char } constantExpression)
            {
                builder.Append('\"');
                builder.Append(constantExpression.Value);
                builder.Append('\"');
            }
        };

        // Act
        StartsWithMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);
        
        // Assert
        Assert.Equal("\"base\".toUpperCase().startsWith(\"start\".toUpperCase())", builder.ToString());
    }
}