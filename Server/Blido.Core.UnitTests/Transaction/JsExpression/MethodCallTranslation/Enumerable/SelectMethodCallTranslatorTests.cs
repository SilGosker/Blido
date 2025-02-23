using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class SelectMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = SelectMethodCallTranslator.SupportedMethods;
        // Act
        var containsNull = supportedMethods.Contains(null);
        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_ShouldAppendMap()
    {
        // Arrange
        var sb = new StringBuilder();
        Expression<Func<int, int>> selector = i => i * 2;
        var method = typeof(System.Linq.Enumerable).GetMethods().First(e => e.Name == nameof(System.Linq.Enumerable.Select)
                                                                            && e.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Func<,>))
            .MakeGenericMethod(typeof(int), typeof(int))!;
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
            else if (next is LambdaExpression)
            {
                sb.Append("i=>i*2");
            }
        };

        // Act
        SelectMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);

        // Assert
        Assert.Equal("[].map(i=>i*2)", sb.ToString());
    }
}