using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Linq;

public class SkipMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = SkipMethodCallTranslator.SupportedMethods;
        
        // Act
        var containsNull = supportedMethods.Contains(null);
        
        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_ShouldAppendSlice()
    {
        // Arrange
        var sb = new StringBuilder();
        var method = typeof(Enumerable).GetMethods().First(e => e.Name == nameof(Enumerable.Skip)
                                                                && e.GetParameters()[1].ParameterType == typeof(int))!
            .MakeGenericMethod(typeof(int));
        var expression = Expression.Call(
            method,
            new Expression[]
            {
                Expression.Constant(new List<int>()),
                Expression.Constant(2)
            }
        );
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression { Value: List<int> })
            {
                sb.Append("[]");
            }
            else if (next is ConstantExpression { Value: int i })
            {
                sb.Append(i);
            }
        };

        // Act
        SkipMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);
        
        // Assert
        Assert.Equal("[].slice(2)", sb.ToString());
    }
}