using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class LastMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = LastMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_WithoutPredicate_ShouldAppendAtNegativeOneIndexExpression()
    {
        // Arrange
        var method = typeof(System.Linq.Enumerable).GetMethods()
            .First(e => e.Name == nameof(System.Linq.Enumerable.Last) && e.GetParameters().Length == 1).MakeGenericMethod(typeof(int));
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
        LastMethodCallTranslator.TranslateMethodCall(sb, expression, processNext);
        // Assert
        Assert.Equal("([1,2,3].at(-1)??throw new Error(\"Sequence contains no matching elements\"))", sb.ToString());
    }

    [Fact]
    public void TranslateMethodCall_WithPredicate_ShouldAppendFindLastWithExpression()
    {
        // Arrange
        var method = typeof(System.Linq.Enumerable).GetMethods()
            .First(e => e.Name == nameof(System.Linq.Enumerable.Last) && e.GetParameters().Length == 2).MakeGenericMethod(typeof(int));
        Expression<Func<int, bool>> predicate = e => e == 2;
        var expression = Expression.Call(method, new Expression[]
        {
            Expression.Constant(new int[] { 1, 2, 3 }),
            predicate
        });
        var sb = new StringBuilder();
        ProcessExpression processNext = next =>
        {
            if (next is ConstantExpression { Value: int[] arr })
            {
                sb.Append('[');
                sb.Append(string.Join(',', arr));
                sb.Append(']');
            }
            else if (next is LambdaExpression)
            {
                sb.Append("e=>e==2");
            }
        };
        // Act
        LastMethodCallTranslator.TranslateMethodCall(sb, expression, processNext);
        // Assert
        Assert.Equal("([1,2,3].findLast(e=>e==2)??throw new Error(\"Sequence contains no matching elements\"))", sb.ToString());
    }
}