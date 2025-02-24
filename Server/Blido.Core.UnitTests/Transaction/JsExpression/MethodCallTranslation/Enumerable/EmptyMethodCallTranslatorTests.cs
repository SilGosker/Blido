using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class EmptyMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = EmptyMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_ShouldAppendEmptyArray()
    {
        // Arrange
        var sb = new StringBuilder();
        var method = typeof(System.Linq.Enumerable).GetMethod(nameof(System.Linq.Enumerable.Empty))!.MakeGenericMethod(typeof(int));
        var expression = Expression.Call(
            method,
            new Expression[] { }
        );

        // Act
        EmptyMethodCallTranslator.TranslateMethodCall(sb, expression, _ => { });
        
        // Assert
        Assert.Equal("[]", sb.ToString());
    }
}