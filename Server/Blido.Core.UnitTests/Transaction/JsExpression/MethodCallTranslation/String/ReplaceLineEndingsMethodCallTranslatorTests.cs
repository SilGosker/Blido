using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class ReplaceLineEndingsMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = ReplaceLineEndingsMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_WithNoArguments_AppendsReplaceAllWithRegex()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.ReplaceLineEndings), Array.Empty<Type>())!;
        var expression = Expression.Call(Expression.Constant("base"), method);
        StringBuilder sb = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression constantExpression)
            {
                sb.Append('\"');
                sb.Append(constantExpression.Value);
                sb.Append('\"');
            }
        };

        // Act
        ReplaceLineEndingsMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);

        // Assert
        Assert.Equal("\"base\".replaceAll(/\\r\\n|\\r|\\n/g,'\\n')", sb.ToString());
    }

    [Fact]
    public void TranslateMethodCall_WithStringArgument_AppendsReplaceAllWithRegexAndArgument()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.ReplaceLineEndings), new Type[] { typeof(string)})!;
        var expression = Expression.Call(Expression.Constant("base"), method, new Expression[] { Expression.Constant("replacement") });
        StringBuilder sb = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression constantExpression)
            {
                sb.Append('\"');
                sb.Append(constantExpression.Value);
                sb.Append('\"');
            }
        };

        // Act
        ReplaceLineEndingsMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);

        // Assert
        Assert.Equal("\"base\".replaceAll(/\\r\\n|\\r|\\n/g,\"replacement\")", sb.ToString());
    }
}