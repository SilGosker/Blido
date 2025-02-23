using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class CompareToMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = CompareToMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_WithStringArgument_AppendsLocaleCompare()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.CompareTo), new[] { typeof(string) })!;
        var builder = new StringBuilder();
        var expression = Expression.Call(Expression.Constant("a"), method,
            new Expression[]
            {
                Expression.Constant("test")
            }
        );

        ProcessExpression processNext = (next) =>
        {
            if (next is ConstantExpression { Value: string s})
            {
                builder.Append('\"');
                builder.Append(s);
                builder.Append('\"');
            }
        };
        
        // Act
        CompareToMethodCallTranslator.TranslateMethodCall(builder, expression, processNext);

        // Assert
        Assert.Equal("\"a\".localCompare(\"test\")", builder.ToString());
    }

    [Fact]
    public void TranslateMethodCall_WithObjectArgument_AppendsLocaleCompare()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.CompareTo), new[] { typeof(object) })!;
        var builder = new StringBuilder();
        var expression = Expression.Call(Expression.Constant("a"), method,
            new Expression[]
            {
                Expression.Constant("test")
            }
        );
        ProcessExpression processNext = (next) =>
        {
            if (next is ConstantExpression { Value: string s })
            {
                builder.Append('\"');
                builder.Append(s);
                builder.Append('\"');
            }
        };

        // Act
        CompareToMethodCallTranslator.TranslateMethodCall(builder, expression, processNext);
        // Assert
        Assert.Equal("\"a\".localCompare(\"test\")", builder.ToString());
    }
}