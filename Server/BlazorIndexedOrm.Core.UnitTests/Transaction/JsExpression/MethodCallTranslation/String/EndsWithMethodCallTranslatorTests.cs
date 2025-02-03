using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class EndsWithMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = EndsWithMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_WithStringArgument_ShouldAppendEndsWithMethodCall()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.EndsWith), new[] { typeof(string) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, new Expression[] { Expression.Constant("end") });

        var builder = new StringBuilder();

        void ProcessExpression(Expression next)
        {
            if (next is ConstantExpression constant)
            {
                builder.Append('\"');
                builder.Append(constant.Value);
                builder.Append('\"');
                return;
            }
        }
        
        // Act
        EndsWithMethodCallTranslator.TranslateMethodCall(builder, expression, ProcessExpression);
        
        // Assert
        Assert.Equal("\"base\".endsWith(\"end\")", builder.ToString());
    }

    [Fact]
    public void TranslateMethodCall_WithCharArgument_ShouldAppendEndsWithMethodCall()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.EndsWith), new[] { typeof(char) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, new Expression[] { Expression.Constant('e') });
        var builder = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression constant)
            {
                builder.Append('\"');
                builder.Append(constant.Value);
                builder.Append('\"');
            }
        };

        // Act
        EndsWithMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);

        // Assert
        Assert.Equal("\"base\".endsWith(\"e\")", builder.ToString());
    }

    [Theory]
    [InlineData(StringComparison.CurrentCultureIgnoreCase)]
    [InlineData(StringComparison.InvariantCultureIgnoreCase)]
    [InlineData(StringComparison.OrdinalIgnoreCase)]
    public void TranslateMethodCall_WithStringComparisonIgnoreCaseArgument_ShouldAppendUpperWithEndsWith(StringComparison comparison)
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.EndsWith), new[] { typeof(string), typeof(StringComparison) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, new Expression[] { Expression.Constant("end"), Expression.Constant(comparison) });
        var builder = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression constant)
            {
                builder.Append('\"');
                builder.Append(constant.Value);
                builder.Append('\"');
            }
        };

        // Act
        EndsWithMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);
        
        // Assert
        Assert.Equal("\"base\".toUpperCase().endsWith(\"end\".toUpperCase())", builder.ToString());
    }

    [Theory]
    [InlineData(StringComparison.CurrentCulture)]
    [InlineData(StringComparison.InvariantCulture)]
    [InlineData(StringComparison.Ordinal)]
    public void TranslateMethodCall_WithStringComparisonArgument_ShouldAppendEndsWith(StringComparison comparison)
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.EndsWith), new[] { typeof(string), typeof(StringComparison) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, new Expression[] { Expression.Constant("end"), Expression.Constant(comparison) });
        var builder = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression constant)
            {
                builder.Append('\"');
                builder.Append(constant.Value);
                builder.Append('\"');
            }
        };

        // Act
        EndsWithMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);

        // Assert
        Assert.Equal("\"base\".endsWith(\"end\")", builder.ToString());
    }
}