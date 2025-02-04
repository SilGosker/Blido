using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class SubStringMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = SubStringMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_WithStartIndex_ShouldAppendSubStringWithStartIndex()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.Substring), new[] { typeof(int) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, new[] { Expression.Constant(1) });
        var builder = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is not ConstantExpression constantExpression)
            {
                return;
            }
            if (constantExpression is { Value: int })
            {
                builder.Append(constantExpression.Value);
            }
            else if (constantExpression is { Value: string or char })
            {
                builder.Append('\"');
                builder.Append(constantExpression.Value);
                builder.Append('\"');
            }
        };

        // Act
        SubStringMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);

        // Assert
        Assert.Equal("\"base\".substring(1)", builder.ToString());
    }

    [Fact]
    public void TranslateMethodCall_WithStartIndexAndLength_ShouldAppendSubStringWithStartIndexAndLength()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.Substring), new[] { typeof(int), typeof(int) })!;
        var expression = Expression.Call(Expression.Constant("base"), method, new Expression[] { Expression.Constant(1), Expression.Constant(2) });
        var builder = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is not ConstantExpression constantExpression)
            {
                return;
            }

            if (constantExpression is { Value: int })
            {
                builder.Append(constantExpression.Value);
            }
            else if (constantExpression is { Value: string or char })
            {
                builder.Append('\"');
                builder.Append(constantExpression.Value);
                builder.Append('\"');
            }
        };
        // Act
        SubStringMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);
        // Assert
        Assert.Equal("\"base\".substring(1, 2)", builder.ToString());
    }
}