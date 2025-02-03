using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class IndexOfAnyMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = IndexOfAnyMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Theory]
    [InlineData(new char[] { 'a', 'b' }, 1, -1)]
    [InlineData(new char[] { 'a', 'b' }, 1, 2)]
    public void TranslateMethodCall_WithUnsupportedMethod_ThrowsNotSupportedException(char[] characters, int start, int end)
    {
        // Arrange
        var arguments = new Type[] { characters.GetType(), start.GetType(), end.GetType() };
        if (end == -1) arguments = new Type[] { characters.GetType(), start.GetType() };
        var parameters = new object[] { characters, start, end };
        if (end == -1) parameters = new object[] { characters, start };

        var method = typeof(string).GetMethod(nameof(string.IndexOfAny), arguments)!;
        var expression = Expression.Call(Expression.Constant("root"), method, parameters.Select(Expression.Constant) );
        var stringBuilder = new StringBuilder();
        ProcessExpression processNext = next => { };

        // Act
        void Act() => IndexOfAnyMethodCallTranslator.TranslateMethodCall(stringBuilder, expression, processNext);

        // Assert
        Assert.Throws<NotSupportedException>(Act);
    }

    [Fact]
    public void TranslateMethodCall_WithCharArray_AppendsFindIndex()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.IndexOfAny), new [] { typeof(char[])})!;
        var expression = Expression.Call(Expression.Constant("root"), method, new Expression[] { Expression.Constant(new char[] {'a', 'b'})});
        var stringBuilder = new StringBuilder();

        ProcessExpression processNext = next =>
        {
            if (next is ConstantExpression { Value: char[] c })
            {
                stringBuilder.Append('[');
                stringBuilder.Append(string.Join(',', c.Select(x => $"'{x}'")));
                stringBuilder.Append(']');
            } else if (next is ConstantExpression { Value: string s })
            {
                stringBuilder.Append('"');
                stringBuilder.Append(s);
                stringBuilder.Append('"');
            }
        };

        // Act
        IndexOfAnyMethodCallTranslator.TranslateMethodCall(stringBuilder, expression, processNext);

        // Assert
        Assert.Equal("\"root\".split('').findIndex(_x=>['a','b'].contains(_x))", stringBuilder.ToString());
    }
}