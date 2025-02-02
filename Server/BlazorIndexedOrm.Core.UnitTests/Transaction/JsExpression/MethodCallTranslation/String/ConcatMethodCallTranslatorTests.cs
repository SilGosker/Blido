using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

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

    [Theory]
    [InlineData("object", "object", "object")]
    [InlineData("object", "object")]
    [InlineData("object")]
    [InlineData("object[]")]
    public void TranslateMethodCall_WithObjectArguments_ThrowsNotSupportedException(params object[] parameters)
    {
        // Arrange
        parameters = parameters.Select(p => p switch
        {
            "object" => new object(),
            "object[]" => Array.Empty<object>(),
            _ => throw new ArgumentOutOfRangeException(nameof(p))
        }).ToArray();
        
        var method = typeof(string).GetMethod(nameof(string.Concat), parameters.Select(p => p.GetType()).ToArray())!;
        var expression = Expression.Call(null, method, parameters.Select(Expression.Constant));
        var builder = new StringBuilder();

        // Act
        Action act = () => ConcatMethodCallTranslator.TranslateMethodCall(builder, expression, _ => { });

        // Assert
        Assert.Throws<NotSupportedException>(act);
    }

    [Fact]
    public void TranslateMethodCall_WithGenericArgument_ThrowsNotSupportedException()
    {
        // Arrange
        var method = typeof(string).GetMethods()
            .Single(m => m is { Name: nameof(string.Concat), IsGenericMethodDefinition: true }
                                 && m.GetParameters() is { Length: 1 } parameters
                                 && parameters[0].ParameterType.IsGenericType
                                 && parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>));

        method = method.MakeGenericMethod(typeof(object));
        var expression = Expression.Call(null, method, Expression.Constant(Enumerable.Empty<object>()));
        var builder = new StringBuilder();

        // Act
        Action act = () => ConcatMethodCallTranslator.TranslateMethodCall(builder, expression, _ => { });

        // Assert
        Assert.Throws<NotSupportedException>(act);
    }

    [Fact]
    public void TranslateMethodCall_WithStringArray_AppendsJoinExpression()
    {
        // Arrange
        var expression = Expression.Call(null, typeof(string).GetMethod(nameof(string.Concat), new[] { typeof(string[]) })!,
            Expression.Constant(new[] { "a", "b", "c" }));
        var builder = new StringBuilder();
        ProcessExpression processExpression = (next) =>
        {
            if (next is not ConstantExpression constantExpression)
            {
                return;
            }

            JsConstantExpressionBuilder.AppendConstant(builder, constantExpression);
        };


        // Act
        ConcatMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);
        
        // Assert
        Assert.Equal("[\"a\",\"b\",\"c\"].join('')", builder.ToString());
    }

    [Fact]
    public void TranslateMethodCall_WithStringEnumerable_AppendsJoinExpression()
    {
        // Arrange
        var expression = Expression.Call(null, typeof(string).GetMethod(nameof(string.Concat), new[] { typeof(IEnumerable<string>) })!,
            Expression.Constant(new[] { "a", "b", "c" }));
        var builder = new StringBuilder();
        ProcessExpression processExpression = (next) =>
        {
            if (next is not ConstantExpression constantExpression)
            {
                return;
            }
            JsConstantExpressionBuilder.AppendConstant(builder, constantExpression);
        };

        // Act
        ConcatMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);

        // Assert
        Assert.Equal("[\"a\",\"b\",\"c\"].join('')", builder.ToString());
    }

    [Theory]
    [InlineData("a", "b")]
    [InlineData("a", "b", "c")]
    [InlineData("a", "b", "c", "d")]
    public void TranslateMethodCall_WithStringArguments_AppendsOperatorExpression(params string?[] parameters)
    {
        // Arrange
        parameters = parameters.Where(e => e != null).ToArray();
        var arguments = parameters.Select(e => e!.GetType()).ToArray();
        var expression = Expression.Call(null, typeof(string).GetMethod(nameof(string.Concat), arguments)!, parameters.Select(Expression.Constant));
        var builder = new StringBuilder();
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression { Value: string s })
            {
                builder.Append('\"');
                builder.Append(s);
                builder.Append('\"');
            }
        };

        // Act
        ConcatMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);

        // Assert
        string expected = $"(\"{string.Join("\"+\"", parameters)}\")";
        Assert.Equal(expected, builder.ToString());
    }
}