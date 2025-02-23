using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class LastIndexOfMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = LastIndexOfMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Theory]
    [InlineData("last")]
    [InlineData('a')]
    public void TranslateMethodCall_WithoutIgnoreCasing_AppendsLastIndexOf(object parameter)
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.LastIndexOf), new[] { parameter.GetType() });
        var expression = Expression.Call(Expression.Constant("base"), method!, Expression.Constant(parameter));
        var stringBuilder = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression constantExpression)
            {
                stringBuilder.Append('\"');
                stringBuilder.Append(constantExpression.Value);
                stringBuilder.Append('\"');
            }
        };
        // Act
        LastIndexOfMethodCallTranslator.TranslateMethodCall(stringBuilder, expression, processExpression);

        // Assert
        Assert.Equal($"\"base\".lastIndexOf(\"{parameter}\")", stringBuilder.ToString());
    }

    [Theory]
    [InlineData(StringComparison.Ordinal)]
    [InlineData(StringComparison.CurrentCulture)]
    [InlineData(StringComparison.InvariantCulture)]
    public void TranslateMethodCall_WithStringComparisonArgument_AppendsLastIndexOf(StringComparison comparison)
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.LastIndexOf), new[] { typeof(string), typeof(StringComparison) });
        var expression = Expression.Call(Expression.Constant("base"), method!, Expression.Constant("last"), Expression.Constant(comparison));
        var stringBuilder = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression constantExpression)
            {
                stringBuilder.Append('\"');
                stringBuilder.Append(constantExpression.Value);
                stringBuilder.Append('\"');
            }
        };

        // Act
        LastIndexOfMethodCallTranslator.TranslateMethodCall(stringBuilder, expression, processExpression);
        
        // Assert
        Assert.Equal("\"base\".lastIndexOf(\"last\")", stringBuilder.ToString());
    }

    [Theory]
    [InlineData(StringComparison.CurrentCultureIgnoreCase)]
    [InlineData(StringComparison.InvariantCultureIgnoreCase)]
    [InlineData(StringComparison.OrdinalIgnoreCase)]
    public void TranslateMethodCall_WithIgnoreCaseStringComparisonArgument_AppendsToUpperCaseAndLastIndexOf(StringComparison comparison)
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.LastIndexOf), new[] { typeof(string), typeof(StringComparison) });
        var expression = Expression.Call(Expression.Constant("base"), method!, Expression.Constant("last"), Expression.Constant(comparison));
        var stringBuilder = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression constantExpression)
            {
                stringBuilder.Append('\"');
                stringBuilder.Append(constantExpression.Value);
                stringBuilder.Append('\"');
            }
        };

        // Act
        LastIndexOfMethodCallTranslator.TranslateMethodCall(stringBuilder, expression, processExpression);

        // Assert
        Assert.Equal("\"base\".toUpperCase().lastIndexOf(\"last\".toUpperCase())", stringBuilder.ToString());
    }

    [Theory]
    [InlineData('a')]
    [InlineData("last")]
    public void TranslateMethodCall_WithIndexArgument_AppendsLastIndexOfWithIndex(object last)
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.LastIndexOf), new[] { last.GetType(), typeof(int) });
        var expression = Expression.Call(Expression.Constant("base"), method!, Expression.Constant(last), Expression.Constant(1));
        var stringBuilder = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is not ConstantExpression constantExpression)
            {
                return;
            }

            if (constantExpression.Value is string or char)
            {
                stringBuilder.Append('\"');
                stringBuilder.Append(constantExpression.Value);
                stringBuilder.Append('\"');
                return;
            }
            stringBuilder.Append(constantExpression.Value);
        };

        // Act
        LastIndexOfMethodCallTranslator.TranslateMethodCall(stringBuilder, expression, processExpression);
        
        // Assert
        Assert.Equal($"\"base\".lastIndexOf(\"{last}\",1)", stringBuilder.ToString());
    }

    [Theory]
    [InlineData(StringComparison.Ordinal)]
    [InlineData(StringComparison.CurrentCulture)]
    [InlineData(StringComparison.InvariantCulture)]
    public void TranslateMethodCall_WithIndexAndStringComparisonArguments_AppendsLastIndexOfWithIndex(StringComparison comparison)
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.LastIndexOf), new[] { typeof(string), typeof(int), typeof(StringComparison) });
        var expression = Expression.Call(Expression.Constant("base"), method!, Expression.Constant("last"), Expression.Constant(1), Expression.Constant(comparison));
        var stringBuilder = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression { Value: string s})
            {
                stringBuilder.Append('\"');
                stringBuilder.Append(s);
                stringBuilder.Append('\"');
            }
            else if (next is ConstantExpression constantExpression)
            {
                stringBuilder.Append(constantExpression.Value);
            }
        };

        // Act
        LastIndexOfMethodCallTranslator.TranslateMethodCall(stringBuilder, expression, processExpression);

        // Assert
        Assert.Equal("\"base\".lastIndexOf(\"last\",1)", stringBuilder.ToString());
    }

    [Theory]
    [InlineData(StringComparison.CurrentCultureIgnoreCase)]
    [InlineData(StringComparison.InvariantCultureIgnoreCase)]
    [InlineData(StringComparison.OrdinalIgnoreCase)]
    public void TranslateMethodCall_WithIndexAndIgnoreCaseStringComparisonArguments_AppendsToUpperCaseAndLastIndexOfWithIndex(StringComparison comparison)
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.LastIndexOf), new[] { typeof(string), typeof(int), typeof(StringComparison) });
        var expression = Expression.Call(Expression.Constant("base"), method!, Expression.Constant("last"), Expression.Constant(1), Expression.Constant(comparison));
        var stringBuilder = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression { Value: string s})
            {
                stringBuilder.Append('\"');
                stringBuilder.Append(s);
                stringBuilder.Append('\"');
            }
            else if (next is ConstantExpression constantExpression)
            {
                stringBuilder.Append(constantExpression.Value);
            }
        };
        // Act
        LastIndexOfMethodCallTranslator.TranslateMethodCall(stringBuilder, expression, processExpression);
        // Assert
        Assert.Equal("\"base\".toUpperCase().lastIndexOf(\"last\".toUpperCase(),1)", stringBuilder.ToString());
    }
}