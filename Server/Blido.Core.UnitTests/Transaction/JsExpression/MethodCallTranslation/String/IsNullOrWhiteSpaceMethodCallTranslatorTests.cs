using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class IsNullOrWhiteSpaceMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = IsNullOrEmptyMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_ShouldAppendTrimAndNotOperator()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.IsNullOrWhiteSpace), new[] { typeof(string) })!;
        var expression = Expression.Call(method, Expression.Constant("test"));
        var builder = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression { Value: string s })
            {
                builder.Append('\"');
                builder.Append(s);
                builder.Append('\"');
            }
        };

        // Act
        IsNullOrWhiteSpaceMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);

        // Assert
        Assert.Equal("(!!\"test\".trim())", builder.ToString());
    }
}