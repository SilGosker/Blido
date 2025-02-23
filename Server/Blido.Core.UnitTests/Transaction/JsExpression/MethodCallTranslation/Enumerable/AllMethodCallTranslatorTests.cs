using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class AllMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = AllMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_ShouldAppendEvery()
    {
        // Arrange
        var sb = new StringBuilder();
        Expression<Func<int, bool>> filter = i => i > 3;
        var method = typeof(System.Linq.Enumerable).GetMethod(nameof(System.Linq.Enumerable.All))!.MakeGenericMethod(typeof(int))!;
        var expression = Expression.Call(
            method,
            new Expression[]
            {
                Expression.Constant(new List<int>()),
                filter
            }
        );

        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression { Value: List<int> })
            {
                sb.Append("[]");
            } else if (next is LambdaExpression)
            {
                sb.Append("i=>i>3");
            }
        };
        
        // Act
        AllMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);
        
        // Assert
        Assert.Equal("[].every(i=>i>3)", sb.ToString());
    }
}