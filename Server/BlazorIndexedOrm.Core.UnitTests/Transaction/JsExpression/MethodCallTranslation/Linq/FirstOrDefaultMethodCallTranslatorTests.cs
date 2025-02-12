using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Linq;

public class FirstOrDefaultMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = FirstOrDefaultMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void SupportedMethods_Length_ShouldBeEqual2()
    {
        // Arrange
        var length = FirstOrDefaultMethodCallTranslator.SupportedMethods.Length;
        var expectedLength = 2;

        // Assert
        Assert.Equal(expectedLength, length);
    }

    [Fact]
    public void TranslateMethodCall_WithoutPredicate_ShouldAppendIndexZeroExpression()
    {
        // Arrange
        var method = typeof(Enumerable).GetMethods()
            .First(e => e.Name == nameof(Enumerable.FirstOrDefault) && e.GetParameters().Length == 1)
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
        FirstOrDefaultMethodCallTranslator.TranslateMethodCall(sb, expression, processNext);

        // Assert
        Assert.Equal("[1,2,3][0]", sb.ToString());
    }

    [Fact]
    public void TranslateMethodCall_WithPredicate_ShouldAppendFindWithExpression()
    {
        // Arrange
        var method = typeof(Enumerable).GetMethods()
            .First(e => e.Name == nameof(Enumerable.FirstOrDefault)
                        && e.GetParameters().Length == 2
                        && e.GetParameters()[1].ParameterType.IsGenericType
                        && e.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Func<,>))
            .MakeGenericMethod(typeof(int));

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
            } else if (next is LambdaExpression lambda)
            {
                sb.Append("e=>e==2");
            }
        };
        // Act
        FirstOrDefaultMethodCallTranslator.TranslateMethodCall(sb, expression, processNext);
        // Assert
        Assert.Equal("[1,2,3].find(e=>e==2)", sb.ToString());
    }
}