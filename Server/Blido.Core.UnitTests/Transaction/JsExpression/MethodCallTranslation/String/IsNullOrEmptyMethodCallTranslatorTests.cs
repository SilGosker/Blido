using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class IsNullOrEmptyMethodCallTranslatorTests
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
    public void TranslateMethodCall_ShouldAppendNotOperator()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.IsNullOrEmpty), new[] { typeof(string) })!;
        var methodCall = Expression.Call(method, Expression.Constant("test"));
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
        IsNullOrEmptyMethodCallTranslator.TranslateMethodCall(builder, methodCall, processExpression);

        // Assert
        Assert.Equal("(!!\"test\")", builder.ToString());
    }

}