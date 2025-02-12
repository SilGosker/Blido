using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class ExceptMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = ExceptMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_ShouldAppendFilterContainsExpression()
    {
        // Arrange
        var sb = new StringBuilder();
        var method = typeof(System.Linq.Enumerable).GetMethods()
            .First(e => e.Name == nameof(System.Linq.Enumerable.Except) && e.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(int));
        var expression = Expression.Call(method,
            new Expression[]
            {
                Expression.Constant(new[] { 1, 2, 3 }),
                Expression.Constant(new[] { 2, 3, 4 })
            }
        );
        ProcessExpression processNext = next =>
        {
            if (next is ConstantExpression { Value: int[] arr })
            {
                sb.Append('[');
                sb.Append(string.Join(',', arr));
                sb.Append(']');
            }
        };

        // Act
        ExceptMethodCallTranslator.TranslateMethodCall(sb, expression, processNext);

        // Assert
        Assert.Equal("[1,2,3].filter(_x=>![2,3,4].contains(_x))", sb.ToString());
    }
}