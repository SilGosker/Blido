using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class ContainsMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = ContainsMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Theory]
    [InlineData("compare")]
    [InlineData('c')]
    public void TranslateMethodCall_WithoutStringComparisonArgument_AppendsInclude(object parameter)
    {
        // Arrange
        var method = typeof(string).GetMethod("Contains", new[]
        {
            parameter.GetType()
        })!;
        var expression = Expression.Call(Expression.Constant("source"), method, Expression.Constant(parameter));
        StringBuilder builder = new StringBuilder();
        ProcessExpression processExpression = (next) =>
        {
            if (next is not ConstantExpression constantExpression)
            {
                return;
            }
            builder.Append('\"');
            builder.Append(constantExpression.Value);
            builder.Append('\"');
        };

        // Act
        ContainsMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);

        // Assert
        Assert.Equal($"\"source\".includes(\"{parameter}\")", builder.ToString());
    }

    [Theory]
    [InlineData(StringComparison.CurrentCulture)]
    [InlineData(StringComparison.InvariantCulture)]
    [InlineData(StringComparison.Ordinal)]
    public void TranslateMethodCall_WithStringComparisonArgument_AppendsInclude(StringComparison comparison)
    {
        // Assert
        var method = typeof(string).GetMethod(nameof(string.Contains),
            new[] { typeof(string), typeof(StringComparison) })!;
        var expression = Expression.Call(Expression.Constant("source"), method, Expression.Constant("compare"), Expression.Constant(comparison));
        StringBuilder builder = new StringBuilder();
        ProcessExpression processExpression = (next) =>
        {
            if (next is not ConstantExpression constantExpression)
            {
                return;
            }
            builder.Append('\"');
            builder.Append(constantExpression.Value);
            builder.Append('\"');
        };

        // Act
        ContainsMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);

        // Assert
        Assert.Equal("\"source\".includes(\"compare\")", builder.ToString());
    }

    [Theory]
    [InlineData(StringComparison.CurrentCultureIgnoreCase)]
    [InlineData(StringComparison.InvariantCultureIgnoreCase)]
    [InlineData(StringComparison.OrdinalIgnoreCase)]
    public void TranslateMethodCall_WithIgnoreCaseStringComparisonArgument_AppendsToUpperAndInclude(StringComparison comparison)
    {
        // Assert
        var method = typeof(string).GetMethod(nameof(string.Contains),
            new[] { typeof(string), typeof(StringComparison) })!;
        var expression = Expression.Call(Expression.Constant("source"), method, Expression.Constant("compare"), Expression.Constant(comparison));
        StringBuilder builder = new StringBuilder();
        ProcessExpression processExpression = (next) =>
        {
            if (next is not ConstantExpression constantExpression)
            {
                return;
            }
            builder.Append('\"');
            builder.Append(constantExpression.Value);
            builder.Append('\"');
        };

        // Act
        ContainsMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);

        // Assert
        Assert.Equal("\"source\".toUpperCase().includes(\"compare\".toUpperCase())", builder.ToString());
    }
}