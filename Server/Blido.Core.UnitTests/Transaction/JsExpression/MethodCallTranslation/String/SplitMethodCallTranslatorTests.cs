using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class SplitMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = SplitMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_WithNoneSplitOptions_AppendsSplit()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.Split), new[] { typeof(char), typeof(StringSplitOptions) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, new Expression[] { Expression.Constant('a'), Expression.Constant(StringSplitOptions.None),  });
        var sb = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression { Value: char or string } constantExpression)
            {
                sb.Append('\"');
                sb.Append(constantExpression.Value);
                sb.Append('\"');
            }
        };

        // Act
        SplitMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);
        
        // Assert
        Assert.Equal("\"base\".split(\"a\")", sb.ToString());
    }

    [Fact]
    public void TranslateMethodCall_WithRemoveEmptySplitOptions_AppendsSplitAndFilter()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.Split), new[] { typeof(char), typeof(StringSplitOptions) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, new Expression[] { Expression.Constant('a'), Expression.Constant(StringSplitOptions.RemoveEmptyEntries) });
        var sb = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression { Value: char or string } constantExpression)
            {
                sb.Append('\"');
                sb.Append(constantExpression.Value);
                sb.Append('\"');
            }
        };

        // Act
        SplitMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);

        // Assert
        Assert.Equal("\"base\".split(\"a\").filter(_x => !!_x)", sb.ToString());
    }

    [Fact]
    public void TranslateMethodCall_WithTrimEntries_AppendsSplitAndMap()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.Split), new[] { typeof(char), typeof(StringSplitOptions) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, new Expression[] { Expression.Constant('a'), Expression.Constant(StringSplitOptions.TrimEntries) });
        var sb = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression { Value: char or string } constantExpression)
            {
                sb.Append('\"');
                sb.Append(constantExpression.Value);
                sb.Append('\"');
            }
        };

        // Act
        SplitMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);
        
        // Assert
        Assert.Equal("\"base\".split(\"a\").map(_x => _x.trim())", sb.ToString());
    }

    [Fact]
    public void TranslateMethodCall_WithTrimEntriesAndRemoveEmpty_AppendsSplitAndMapAndFilter()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.Split), new[] { typeof(char), typeof(StringSplitOptions) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, new Expression[] { Expression.Constant('a'), Expression.Constant(StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries) });
        var sb = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression { Value: char or string } constantExpression)
            {
                sb.Append('\"');
                sb.Append(constantExpression.Value);
                sb.Append('\"');
            }
        };

        // Act
        SplitMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);
        
        // Assert
        Assert.Equal("\"base\".split(\"a\").map(_x => _x.trim()).filter(_x => !!_x)", sb.ToString());
    }

}