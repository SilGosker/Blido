using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class ConcatMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = ConcatMethodCallTranslator.SupportedMethods;
        
        // Act
        var containsNull = supportedMethods.Contains(null);
        
        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_ShouldAppendConcat()
    {
        // Arrange
        var sb = new StringBuilder();
        var method = typeof(System.Linq.Enumerable).GetMethod(nameof(System.Linq.Enumerable.Concat))!.MakeGenericMethod(typeof(int))!;
        var expression = Expression.Call(
            method,
            new Expression[]
            {
                Expression.Constant(new List<int>()),
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
        ConcatMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);
        
        // Assert
        Assert.Equal("[].concat([])", sb.ToString());
    }
}