using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Linq;

public class SumMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = SumMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_WithoutSelector_ShouldAppendReduce()
    {
        // Arrange
        var sb = new StringBuilder();
        var method = typeof(Enumerable).GetMethods().First(e => e.Name == nameof(Enumerable.Sum));
        var expression = Expression.Call(
            method,
            new Expression[]
            {
                Expression.Constant(new List<int>())
            }
        );
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression { Value: List<int> })
            {
                sb.Append("[]");
            }
        };

        // Act
        SumMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);

        // Assert
        Assert.Equal("[].reduce((a,b)=>a+b,0)", sb.ToString());
    }

    [Fact]
    public void TranslateMethodCall_WithSelector_ShouldAppendMapAndReduce()
    {
        // Arrange
        var sb = new StringBuilder();
        var method = typeof(Enumerable).GetMethods().First(e => e.Name == nameof(Enumerable.Sum) && e.IsGenericMethodDefinition)!
            .MakeGenericMethod(typeof(int));
        Expression<Func<int, int>> selector = i => 1;
        var expression = Expression.Call(
            method,
            new Expression[]
            {
                Expression.Constant(new List<int>()),
                selector
            }
        );
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression { Value: List<int> })
            {
                sb.Append("[]");
            }
            else if (next is LambdaExpression { Body: ConstantExpression { Value: 1 } })
            {
                sb.Append("i=>1");
            }
        };

        // Act
        SumMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);
        
        // Assert
        Assert.Equal("[].map(i=>1).reduce((a,b)=>a+b,0)", sb.ToString());
    }
}