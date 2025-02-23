using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class LastOrDefaultMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = LastOrDefaultMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);
        
        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void SupportedMethods_Length_ShouldBeEqual2()
    {
        // Arrange
        var length = LastOrDefaultMethodCallTranslator.SupportedMethods.Length;
        var expectedLength = 2;

        // Assert
        Assert.Equal(expectedLength, length);
    }

    [Fact]
    public void TranslateMethodCall_WithoutPredicate_ShouldAppendAtMinusOneExpression()
    {
        // Arrange
        var method = typeof(System.Linq.Enumerable).GetMethods()
            .First(e => e.Name == nameof(System.Linq.Enumerable.LastOrDefault) && e.GetParameters().Length == 1)
            .MakeGenericMethod(typeof(int));
        var expression = Expression.Call(method, new Expression[] { Expression.Constant(new int[] { 1, 2, 3 }) });
        var sb = new StringBuilder();
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
        LastOrDefaultMethodCallTranslator.TranslateMethodCall(sb, expression, processNext);
        
        // Assert
        Assert.Equal("[1,2,3].at(-1)", sb.ToString());
    }

    [Fact]
    public void TranslateMethodCall_WithPredicate_ShouldAppendFindLastWithExpression()
    {
        // Arrange
        var method = typeof(System.Linq.Enumerable).GetMethods()
            .First(e => e.Name == nameof(System.Linq.Enumerable.LastOrDefault)
                        && e.GetParameters().Length == 2
                        && e.GetParameters()[1].ParameterType.IsGenericType
                        && e.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Func<,>))
            .MakeGenericMethod(typeof(int));
        Expression<Func<int, bool>> predicate = x => x == 2;
        var expression = Expression.Call(method, new Expression[] { Expression.Constant(new int[] { 1, 2, 3 }), predicate });
        var sb = new StringBuilder();
        ProcessExpression processNext = next =>
        {
            if (next is ConstantExpression { Value: int[] arr })
            {
                sb.Append('[');
                sb.Append(string.Join(',', arr));
                sb.Append(']');
            } else if (next is LambdaExpression)
            {
                sb.Append("x => x == 2");
            }
        };

        // Act
        LastOrDefaultMethodCallTranslator.TranslateMethodCall(sb, expression, processNext);
        
        // Assert
        Assert.Equal("[1,2,3].findLast(x => x == 2)", sb.ToString());
    }
}