using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.Enumerable;

public class ElementAtMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = ElementAtMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_ShouldAppendIndexExpression()
    {
        // Arrange
        var sb = new StringBuilder();
        var method = typeof(System.Linq.Enumerable).GetMethods().First(e => e.Name == nameof(System.Linq.Enumerable.ElementAt))
            .MakeGenericMethod(typeof(int));
        var expression = Expression.Call(method,
            new Expression[]
            {
                Expression.Constant(new int[] { 1, 2, 3 }),
                Expression.Constant(1)
            });

        void ProcessNext(Expression next)
        {
            if (next is ConstantExpression { Value: int[] arr })
            {
                sb.Append('[');
                sb.Append(string.Join(',', arr));
                sb.Append(']');
            }
            else
            {
                sb.Append(1);
            }
        }

        // Act
        ElementAtMethodCallTranslator.TranslateMethodCall(sb, expression, ProcessNext);
        
        // Assert
        Assert.Equal("([1,2,3][1]??throw new Error(\"Index was out of range\"))", sb.ToString());
    }
}