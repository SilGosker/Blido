using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression;

public class JsParameterExpressionBuilderTests
{
    [Fact]
    public void AppendParameter_AppendsParameterName()
    {
        // Arrange
        var stringBuilder = new StringBuilder();
        var parameter = Expression.Parameter(typeof(int), "x");
        var processExpression = new ProcessExpression(_ => { });

        // Act
        JsParameterExpressionBuilder.AppendParameter(stringBuilder, parameter, processExpression);

        // Assert
        Assert.Equal("x", stringBuilder.ToString());
    }
}