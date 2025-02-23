using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class TrimEndMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = TrimEndMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_ShouldAppendTrimEnd()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.TrimEnd), Array.Empty<Type>())!;
        var expression = Expression.Call(Expression.Constant("base"), method);
        var stringBuilder = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression { Value: string s })
            {
                stringBuilder.Append('\"');
                stringBuilder.Append(s);
                stringBuilder.Append('\"');
            }
        };

        // Act
        TrimEndMethodCallTranslator.TranslateMethodCall(stringBuilder, expression, processExpression);
        
        // Assert
        Assert.Equal("\"base\".trimEnd()", stringBuilder.ToString());
    }
}