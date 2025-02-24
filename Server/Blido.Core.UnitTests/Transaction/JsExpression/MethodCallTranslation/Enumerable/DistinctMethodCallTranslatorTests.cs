using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class DistinctMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = DistinctMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);
        
        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_ShouldAppendFilter()
    {
        // Arrange
        var sb = new StringBuilder();
        var method = typeof(System.Linq.Enumerable).GetMethods().First(x =>
            x.Name == nameof(System.Linq.Enumerable.Distinct)
            && x.GetParameters().Length == 1)!.MakeGenericMethod(typeof(int))!;
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
        DistinctMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);

        // Assert
        Assert.Equal("[].filter((_v, _i, _a)=>_a.indexOf(_v)===_i)", sb.ToString());
    }
}