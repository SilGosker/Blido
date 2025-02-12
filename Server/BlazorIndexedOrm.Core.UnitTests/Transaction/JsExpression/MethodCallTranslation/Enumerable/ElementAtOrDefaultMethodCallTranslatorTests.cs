using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class ElementAtOrDefaultMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = ElementAtOrDefaultMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_ShouldAppendIndexExpression()
    {
        // Arrange
        var method = typeof(System.Linq.Enumerable).GetMethods()
            .First(e => e.Name == nameof(System.Linq.Enumerable.ElementAtOrDefault))
            .MakeGenericMethod(typeof(int));
        var expression = Expression.Call(method,
            new Expression[]
            {
                Expression.Constant(new int[] { 1, 2, 3 }),
                Expression.Constant(1)
            }
        );
        var sb = new StringBuilder();
        ProcessExpression processNext = e =>
        {
            if (e is ConstantExpression { Value: int[] arr })
            {
                sb.Append('[');
                sb.Append(string.Join(',', arr));
                sb.Append(']');
            } else if (e is ConstantExpression { Value: int i })
            {
                sb.Append(i);
            }
        };

        // Act
        ElementAtOrDefaultMethodCallTranslator.TranslateMethodCall(sb, expression, processNext);
        
        // Assert
        Assert.Equal("[1,2,3][1]", sb.ToString());
    }
}