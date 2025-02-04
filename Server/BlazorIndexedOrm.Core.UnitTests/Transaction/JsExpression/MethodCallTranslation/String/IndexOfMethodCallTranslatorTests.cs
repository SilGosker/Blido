using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class IndexOfMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = IndexOfMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_WithString_AppendsIndexOf()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.IndexOf), new[] { typeof(string) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, Expression.Constant("arg"));
        var sb = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is not ConstantExpression constantExpression)
            {
                return;
            }
            sb.Append('\"');
            sb.Append(constantExpression.Value);
            sb.Append('\"');
        };

        // Act
        IndexOfMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);
        
        // Assert
        Assert.Equal("\"base\".indexOf(\"arg\")", sb.ToString());
    }

    [Fact]
    public void TranslateMethodCall_WithStringAndIndex_AppendsIndexOf()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.IndexOf), new[] { typeof(string), typeof(int) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, Expression.Constant("arg"), Expression.Constant(1));
        var sb = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is not ConstantExpression constantExpression)
            {
                return;
            }

            if (constantExpression.Value is int i)
            {
                sb.Append(i);
                return;
            }

            sb.Append('\"');
            sb.Append(constantExpression.Value);
            sb.Append('\"');
        };

        // Act
        IndexOfMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);

        // Assert
        Assert.Equal("\"base\".indexOf(\"arg\",1)", sb.ToString());
    }

    [Theory]
    [InlineData(StringComparison.CurrentCulture)]
    [InlineData(StringComparison.InvariantCulture)]
    [InlineData(StringComparison.Ordinal)]
    public void TranslateMethodCall_WithComparison_AppendsIndexOf(StringComparison comparison)
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.IndexOf), new[] { typeof(string), typeof(StringComparison) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, Expression.Constant("arg"), Expression.Constant(comparison));
        var sb = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is not ConstantExpression constantExpression)
            {
                return;
            }
            if (constantExpression.Value is int i)
            {
                sb.Append(i);
                return;
            }
            sb.Append('\"');
            sb.Append(constantExpression.Value);
            sb.Append('\"');
        };

        // Act
        IndexOfMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);

        // Assert
        Assert.Equal("\"base\".indexOf(\"arg\")", sb.ToString());
    }

    [Theory]
    [InlineData(StringComparison.CurrentCultureIgnoreCase)]
    [InlineData(StringComparison.InvariantCultureIgnoreCase)]
    [InlineData(StringComparison.OrdinalIgnoreCase)]
    public void TranslateMethodCall_WithIgnoreCaseStringComparison_AppendsToUpperCaseAndIndexOf(StringComparison comparison)
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.IndexOf), new[] { typeof(string), typeof(StringComparison) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, Expression.Constant("arg"), Expression.Constant(comparison));
        var sb = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is not ConstantExpression constantExpression)
            {
                return;
            }
            sb.Append('\"');
            sb.Append(constantExpression.Value);
            sb.Append('\"');
        };

        // Act
        IndexOfMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);
        
        // Assert
        Assert.Equal("\"base\".toUpperCase().indexOf(\"arg\".toUpperCase())", sb.ToString());
    }

    [Theory]
    [InlineData(StringComparison.CurrentCulture)]
    [InlineData(StringComparison.InvariantCulture)]
    [InlineData(StringComparison.Ordinal)]
    public void TranslateMethodCall_WithStringAndIndexAndComparison_AppendsIndexOf(StringComparison comparison)
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.IndexOf), new[] { typeof(string), typeof(int), typeof(StringComparison) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, Expression.Constant("arg"), Expression.Constant(1), Expression.Constant(comparison));
        var sb = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is not ConstantExpression constantExpression)
            {
                return;
            }
            if (constantExpression.Value is int i)
            {
                sb.Append(i);
                return;
            }
            sb.Append('\"');
            sb.Append(constantExpression.Value);
            sb.Append('\"');
        };

        // Act
        IndexOfMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);
        
        // Assert
        Assert.Equal("\"base\".indexOf(\"arg\",1)", sb.ToString());
    }

    [Theory]
    [InlineData(StringComparison.CurrentCultureIgnoreCase)]
    [InlineData(StringComparison.InvariantCultureIgnoreCase)]
    [InlineData(StringComparison.OrdinalIgnoreCase)]
    public void TranslateMethodCall_WithIndexAndIgnoreCaseStringComparison_AppendsToUpperCaseAndIndexOf(
        StringComparison comparison)
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.IndexOf), new[] { typeof(string), typeof(StringComparison) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, Expression.Constant("arg"), Expression.Constant(comparison));
        var sb = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is not ConstantExpression constantExpression)
            {
                return;
            }
            sb.Append('\"');
            sb.Append(constantExpression.Value);
            sb.Append('\"');
        };

        // Act
        IndexOfMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);

        // Assert
        Assert.Equal("\"base\".toUpperCase().indexOf(\"arg\".toUpperCase())", sb.ToString());
    }
}