using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class AnyMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = AnyMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }


    [Fact]
    public void TranslateMethodCall_ShouldAppendSome()
    {
        // Arrange
        var sb = new StringBuilder();
        Expression<Func<int, bool>> filter = i => i > 3;
        var method = typeof(System.Linq.Enumerable).GetMethod(nameof(System.Linq.Enumerable.All))!.MakeGenericMethod(typeof(int))!;
        var expression = Expression.Call(
            method,
            new Expression[]
            {
                Expression.Constant(new List<int>()),
                filter
            }
        );

        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression { Value: List<int> })
            {
                sb.Append("[]");
            }
            else if (next is LambdaExpression)
            {
                sb.Append("i=>i>3");
            }
        };

        // Act
        AnyMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);

        // Assert
        Assert.Equal("[].some(i=>i>3)", sb.ToString());
    }
}