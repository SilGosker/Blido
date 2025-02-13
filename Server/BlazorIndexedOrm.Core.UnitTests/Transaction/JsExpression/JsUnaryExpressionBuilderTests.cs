using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression;

public class JsUnaryExpressionBuilderTests
{
    [Fact]
    public void AppendUnary_WithNegation_AppendsNegationSymbol()
    {
        // Arrange
        var sb = new StringBuilder();
        var expression = Expression.Not(Expression.Constant(false));
        void ProcessExpression(Expression e) => sb.Append(e.ToString());

        // Act
        JsUnaryExpressionBuilder.AppendUnary(sb, expression, ProcessExpression);

        // Assert
        Assert.Equal("!(False)", sb.ToString());
    }

    [Fact]
    public void AppendUnary_WithComplement_AppendsComplementSymbol()
    {
        // Arrange
        var sb = new StringBuilder();
        var expression = Expression.OnesComplement(Expression.Constant(42));
        void ProcessExpression(Expression e) => sb.Append(e.ToString());

        // Act
        JsUnaryExpressionBuilder.AppendUnary(sb, expression, ProcessExpression);

        // Assert
        Assert.Equal("~(42)", sb.ToString());
    }

    [Fact]
    public void AppendUnary_WithMinus_AppendsMinusSymbol()
    {
        // Arrange
        var sb = new StringBuilder();
        var expression = Expression.Negate(Expression.Constant(42));
        void ProcessExpression(Expression e) => sb.Append(e.ToString());

        // Act
        JsUnaryExpressionBuilder.AppendUnary(sb, expression, ProcessExpression);
        
        // Assert
        Assert.Equal("-(42)", sb.ToString());
    }

    [Fact]
    public void AppendOperand_WithArrayLength_AppendsLength()
    {
        // Arrange
        var sb = new StringBuilder();
        var expression = Expression.ArrayLength(Expression.Constant(new[] { 1, 2, 3 }));
        void ProcessExpression(Expression e)
        {
            if (e is ConstantExpression { Value: int[] arr })
            {
                sb.Append('[');
                sb.Append(string.Join(',', arr));
                sb.Append(']');
            }
        }

        // Act
        JsUnaryExpressionBuilder.AppendUnary(sb, expression, ProcessExpression);
        
        // Assert
        Assert.Equal("[1,2,3].length", sb.ToString());
    }
}