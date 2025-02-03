using Microsoft.Extensions.Primitives;
using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class PadRightMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = PadRightMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethod_WithIntArgument_ShouldAppendPadEnd()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.PadRight), new[] { typeof(int) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, Expression.Constant(5));
        var sb = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression { Value: string s })
            {
                sb.Append('"');
                sb.Append(s);
                sb.Append('"');
                return;
            }
            if (next is ConstantExpression constantExpression)
            {
                sb.Append(constantExpression.Value);
            }
        };

        // Act
        PadRightMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);

        // Assert
        Assert.Equal("\"base\".padEnd(5)", sb.ToString());
    }

    [Fact]
    public void TranslateMethodCall_WithIntAndCharArgument_ShouldAppendPadEnd()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.PadRight), new[] { typeof(int), typeof(char) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, Expression.Constant(5), Expression.Constant('0'));
        var sb = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is not ConstantExpression constantExpression)
            {
                return;
            }

            if (constantExpression is { Value: string or char })
            {
                sb.Append('"');
                sb.Append(constantExpression.Value);
                sb.Append('"');
                return;
            }
            sb.Append(constantExpression.Value);
        };

        // Act
        PadRightMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);

        // Assert
        Assert.Equal("\"base\".padEnd(5,\"0\")", sb.ToString());
    }
}
