using System.Globalization;
using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class ToLowerMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = ToLowerMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_WithNoArguments_ShouldAppendToLowerCase()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.ToLower), Array.Empty<Type>())!;
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
        ToLowerMethodCallTranslator.TranslateMethodCall(stringBuilder, expression, processExpression);
        // Assert
        Assert.Equal("\"base\".toLowerCase()", stringBuilder.ToString());
    }

    [Fact]
    public void TranslateMethodCall_WithCultureInfo_ShouldAppendToLocalLowerCase()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.ToLower), new Type[] { typeof(CultureInfo) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, new Expression[] { Expression.Constant(new CultureInfo("en-US")) });
        var stringBuilder = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression { Value: string s })
            {
                stringBuilder.Append('\"');
                stringBuilder.Append(s);
                stringBuilder.Append('\"');
            } else if (next is ConstantExpression { Value: CultureInfo cultureInfo })
            {
                stringBuilder.Append('\"');
                stringBuilder.Append(cultureInfo.TwoLetterISOLanguageName);
                stringBuilder.Append('\"');
            }
        };

        // Act
        ToLowerMethodCallTranslator.TranslateMethodCall(stringBuilder, expression, processExpression);

        // Assert
        Assert.Equal("\"base\".toLocalLowerCase(\"en\")", stringBuilder.ToString());
    }
}