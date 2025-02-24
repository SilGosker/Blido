using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class CountMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = CountMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_WithCollection_ShouldAppendLength()
    {
        // Arrange
        var method = typeof(System.Linq.Enumerable).GetMethods().First(e => e.Name == nameof(System.Linq.Enumerable.Count)
                                                                            && e.GetParameters().Length == 1).MakeGenericMethod(typeof(int));
        var expression = Expression.Call(
            method,
            new Expression[]
            {
                Expression.Constant(new List<int>())
            });

        var sb = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            sb.Append("[]");
        };
        
        // Act
        CountMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);
        
        // Assert
        Assert.Equal("[].length", sb.ToString());
    }

    [Fact]
    public void TranslateMethodCall_WithExpression_ShouldAppendReduce()
    {
        // Arrange
        var method = typeof(System.Linq.Enumerable).GetMethods().First(e => e.Name == nameof(System.Linq.Enumerable.Count)
                                                                            && e.GetParameters().Length == 2).MakeGenericMethod(typeof(int));
        var expression = Expression.Call(
            method,
            new Expression[]
            {
                Expression.Constant(new List<int>()),
                (Expression<Func<int, bool>>)(i => i > 0)
            });
        var sb = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression)
            {
                sb.Append("[]");
            }
            else
            {
                sb.Append("i=>i>0");
            }
        };

        // Act
        CountMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);

        // Assert
        Assert.Equal("[].reduce((_t,_e)=>((i=>i>0)(_e)?_t+1:_t).length", sb.ToString());
    }
}