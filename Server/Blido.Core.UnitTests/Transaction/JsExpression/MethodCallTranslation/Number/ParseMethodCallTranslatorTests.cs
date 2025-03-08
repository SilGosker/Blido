using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.Number;

public class ParseMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = ParseMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_ShouldAppendBigInt()
    {
        // Arrange
        var methodInfo = typeof(ulong).GetMethod(nameof(ulong.Parse), new Type[] { typeof(string) })!;
        var methodCall = Expression.Call(methodInfo, Expression.Constant("123"));
        StringBuilder sb = new StringBuilder();
        ProcessExpression processNext = expression => sb.Append(expression);

        // Act
        ParseMethodCallTranslator.TranslateMethodCall(sb, methodCall, processNext);

        // Assert
        Assert.Equal("parseInt(\"123\")", sb.ToString());
    }
}