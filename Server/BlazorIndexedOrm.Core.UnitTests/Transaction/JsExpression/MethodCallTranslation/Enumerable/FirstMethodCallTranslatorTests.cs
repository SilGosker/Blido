using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class FirstMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = FirstMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_WithoutArguments_ShouldAppendZeroIndexExpression()
    {
        // Arrange
        var method = typeof(System.Linq.Enumerable).GetMethods()
            .First(e => e.Name == nameof(System.Linq.Enumerable.First) && e.GetParameters().Length == 1).MakeGenericMethod(typeof(int));
        var expression = Expression.Call(method, new Expression[] { Expression.Constant(new int[] { 1, 2, 3}) });
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
        FirstMethodCallTranslator.TranslateMethodCall(sb, expression, processNext);

        // Assert
        Assert.Equal("([1,2,3][0]??throw new Error(\"Sequence contains no (matching) elements\"))", sb.ToString());
    }

    [Fact]
    public void TranslateMethodCall_WithArguments_ShouldAppendFindExpression()
    {
        // Arrange
        var method = typeof(System.Linq.Enumerable).GetMethods()
            .First(e => e.Name == nameof(System.Linq.Enumerable.First) && e.GetParameters().Length == 2).MakeGenericMethod(typeof(int));
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
            else if (next is Expression<Func<int, bool>> { Body: BinaryExpression { Left: ParameterExpression { Name: "e" }, Right: ConstantExpression { Value: 2 } } })
            {
                sb.Append("e=>e==2");
            }
        };
        
        // Act
        FirstMethodCallTranslator.TranslateMethodCall(sb, expression, processNext);

        // Assert
        Assert.Equal("([1,2,3].find(e=>e==2)??throw new Error(\"Sequence contains no (matching) elements\"))", sb.ToString());
    }
}