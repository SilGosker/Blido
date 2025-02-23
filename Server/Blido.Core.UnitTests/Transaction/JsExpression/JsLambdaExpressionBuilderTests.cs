using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression;

public class JsLambdaExpressionBuilderTests
{
    [Fact]
    public void AppendLambda_ShouldAppendFunctionStart()
    {
        // Arrange
        var sb = new StringBuilder();
        LambdaExpression expression = ((Expression<Func<string, string?>>)(a => null));
        ProcessExpression processExpression = parameter =>
        {
            if (parameter is ParameterExpression parameterExpression)
            {
                sb.Append(parameterExpression.Name);
            }
        };


        // Act
        JsLambdaExpressionBuilder.AppendLambda(sb, expression, processExpression);

        // Assert
        Assert.Equal("(a)=>", sb.ToString());
    }

    [Fact]
    public void AppendLambda_WithMultipleArguments_ShouldAppendArguments()
    {
        // Arrange
        var sb = new StringBuilder();
        LambdaExpression expression = ((Expression<Func<string, string, string?>>)((a, b) => null));
        ProcessExpression processExpression = parameter =>
        {
            if (parameter is ParameterExpression parameterExpression)
            {
                sb.Append(parameterExpression.Name);
            }
        };

        // Act
        JsLambdaExpressionBuilder.AppendLambda(sb, expression, processExpression);

        // Assert
        Assert.Equal("(a,b)=>", sb.ToString());
    }
}