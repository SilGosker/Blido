using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class EqualsMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = EqualsMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_Object_WithoutStringComparisonArgument_ShouldAppendEqualityOperator()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.Equals), new[] { typeof(string) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, new Expression[] { Expression.Constant("comparison") });
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
        EqualsMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);

        // Assert
        Assert.Equal("(\"base\"===\"comparison\")", builder.ToString());
    }

    [Theory]
    [InlineData(StringComparison.CurrentCultureIgnoreCase)]
    [InlineData(StringComparison.InvariantCultureIgnoreCase)]
    [InlineData(StringComparison.OrdinalIgnoreCase)]
    public void TranslateMethodCall_Object_WithStringComparisonIgnoreCaseArgument_ShouldAppendLocaleCompareEquals0(StringComparison comparison)
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.Equals), new[] { typeof(string), typeof(StringComparison) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, new Expression[] { Expression.Constant("comparison"), Expression.Constant(comparison) });
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
        EqualsMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);
        
        // Assert
        Assert.Equal("(\"base\".localeCompare(\"comparison\",undefined,{sensitivity:'accent'})===0)", builder.ToString());
    }

    [Theory]
    [InlineData(StringComparison.CurrentCulture)]
    [InlineData(StringComparison.InvariantCulture)]
    [InlineData(StringComparison.Ordinal)]
    public void TranslateMethodCall_Object_WithStringComparisonArgument_ShouldAppendEqualityOperator(StringComparison comparison)
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.Equals), new[] { typeof(string), typeof(StringComparison) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, new Expression[] { Expression.Constant("comparison"), Expression.Constant(comparison) });
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
        EqualsMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);

        // Assert
        Assert.Equal("(\"base\"===\"comparison\")", builder.ToString());
    }

    [Fact]
    public void TranslateMethodCall_Argument_WithoutStringComparisonArgument_ShouldAppendEqualityOperator()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.Equals), new[] { typeof(string), typeof(string) })!;
        var expression = Expression.Call(null, method, new Expression[] { Expression.Constant("base"), Expression.Constant("comparison") });
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
        EqualsMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);
        // Assert
        Assert.Equal("(\"base\"===\"comparison\")", builder.ToString());
    }

    [Theory]
    [InlineData(StringComparison.CurrentCultureIgnoreCase)]
    [InlineData(StringComparison.InvariantCultureIgnoreCase)]
    [InlineData(StringComparison.OrdinalIgnoreCase)]
    public void TranslateMethodCall_Argument_WithStringComparisonIgnoreCaseArgument_ShouldAppendLocaleCompareEquals0(StringComparison comparison)
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.Equals), new[] { typeof(string), typeof(string), typeof(StringComparison) })!;
        var expression = Expression.Call(null, method, new Expression[] { Expression.Constant("base"), Expression.Constant("comparison"), Expression.Constant(comparison) });
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
        EqualsMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);

        // Assert
        Assert.Equal("(\"base\".localeCompare(\"comparison\",undefined,{sensitivity:'accent'})===0)", builder.ToString());
    }

    [Theory]
    [InlineData(StringComparison.CurrentCulture)]
    [InlineData(StringComparison.InvariantCulture)]
    [InlineData(StringComparison.Ordinal)]
    public void TranslateMethodCall_Argument_WithStringComparisonArgument_ShouldAppendEqualityOperator(StringComparison comparison)
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.Equals),  new[] { typeof(string), typeof(string), typeof(StringComparison) })!;
        var expression = Expression.Call(null, method, new Expression[] { Expression.Constant("base"), Expression.Constant("comparison"), Expression.Constant(comparison) });
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
        EqualsMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);
        
        // Assert
        Assert.Equal("(\"base\"===\"comparison\")", builder.ToString());
    }
}