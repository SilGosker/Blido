using System.Globalization;
using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class ReplaceMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = ReplaceMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Theory]
    [InlineData("test", "test", true, "en-US")]
    [InlineData("test", "test", StringComparison.CurrentCulture)]
    public void TranslateMethodCall_WithUnsupportedMethod_ShouldThrowNotSupportedException(params object[] arguments)
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.Replace), arguments.Select(e => e is "en-US" ? typeof(CultureInfo) : e.GetType()).ToArray())!;
        var expression = Expression.Call(Expression.Constant("base"), method, arguments.Select(e => Expression.Constant(e is "en-US" ? new CultureInfo((string)e) : e)));
        var sb = new StringBuilder();
        ProcessExpression processExpression = next => { };

        // Act
        Action action = () => ReplaceMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);
        
        // Assert
        Assert.Throws<NotSupportedException>(action);
    }

    [Fact]
    public void TranslateMethodCall_WithString_ShouldAppendReplaceAll()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.Replace), new[] { typeof(string), typeof(string) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, new Expression[] {  Expression.Constant("search"), Expression.Constant("replacement") });
        var sb = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression { Value: string or char } constantExpression)
            {
                sb.Append('\"');
                sb.Append(constantExpression.Value);
                sb.Append('\"');
            }
        };

        // Act
        ReplaceMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);

        // Assert
        Assert.Equal("\"base\".replaceAll(\"search\",\"replacement\")", sb.ToString());
    }
}