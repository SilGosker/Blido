using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class TakeMethodCallTranslationTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = TakeMethodCallTranslation.SupportedMethods;

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
        var method = typeof(System.Linq.Enumerable).GetMethods()
            .First(e => e.Name == nameof(System.Linq.Enumerable.Take)
                        && e.GetParameters().Length == 2
                        && e.GetParameters()[1].ParameterType == typeof(int))
            .MakeGenericMethod(typeof(int));

        var expression = Expression.Call(
            method,
            new Expression[]
            {
                Expression.Constant(new List<int>()),
                Expression.Constant(1)
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
        TakeMethodCallTranslation.TranslateMethodCall(sb, expression, processExpression);
        
        // Assert
        Assert.Equal("[].slice(0,1)", sb.ToString());
    }
}