using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class IndexAtMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = IndexAtMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_ShouldAppendCharAt()
    {
        // Arrange
        var method = typeof(string).GetMethod("get_Chars", new[] { typeof(int) })!;
        var expression = Expression.Call(Expression.Constant("test"), method, new Expression[] { Expression.Constant(0) });
        var builder = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression { Value: string s })
            {
                builder.Append('\"');
                builder.Append(s);
                builder.Append('\"');
            }
            else if (next is ConstantExpression { Value: int i })
            {
                builder.Append(i);
            }
        };

        // Act
        IndexAtMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);

        // Assert
        Assert.Equal("\"test\".charAt(0)", builder.ToString());
    }
}