using System.Globalization;
using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class ToUpperMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = ToUpperMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_WithNoArguments_ShouldAppendToUpperCase()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.ToUpper), Array.Empty<Type>())!;
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
        ToUpperMethodCallTranslator.TranslateMethodCall(stringBuilder, expression, processExpression);
        // Assert
        Assert.Equal("\"base\".toUpperCase()", stringBuilder.ToString());
    }

    [Fact]
    public void TranslateMethodCall_WithCultureInfo_ShouldAppendToLocalUpperCase()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.ToUpper), new Type[] { typeof(CultureInfo) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, new Expression[] { Expression.Constant(new CultureInfo("en-US")) });
        var stringBuilder = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression { Value: string s })
            {
                stringBuilder.Append('\"');
                stringBuilder.Append(s);
                stringBuilder.Append('\"');
            }
            else if (next is ConstantExpression { Value: CultureInfo cultureInfo })
            {
                stringBuilder.Append('\"');
                stringBuilder.Append(cultureInfo.TwoLetterISOLanguageName);
                stringBuilder.Append('\"');
            }
        };

        // Act
        ToUpperMethodCallTranslator.TranslateMethodCall(stringBuilder, expression, processExpression);

        // Assert
        Assert.Equal("\"base\".toLocaleUpperCase(\"en\")", stringBuilder.ToString());
    }
}
