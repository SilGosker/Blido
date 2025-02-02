using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression;

public class JsBinaryExpressionBuilderTests
{
    [Fact]
    public void Constructor_ShouldNotThrow()
    {
        // Arrange, Act
        JsBinaryExpressionBuilder? jsBinaryExpressionBuilder = new JsBinaryExpressionBuilder();

        // Assert
        Assert.NotEqual(null!, jsBinaryExpressionBuilder);
    }

    [Fact]
    public void AppendEquality_WithEqualExpression_AppendsExpression()
    {
        // Arrange
        var expression = Expression.Equal(Expression.Constant(10), Expression.Constant(20));
        StringBuilder sb = new();
        ProcessExpression processExpression = (ex) =>
        {
            sb.Append(((ConstantExpression)ex).Value);
        };

        // Act
        JsBinaryExpressionBuilder.AppendEquality(sb, expression, processExpression);

        // Assert
        Assert.Equal("(10)===(20)", sb.ToString());
    }

    [Fact]
    public void AppendEquality_WithNotEqualExpression_AppendsExpression()
    {
        // Arrange
        var expression = Expression.NotEqual(Expression.Constant(10), Expression.Constant(20));
        StringBuilder sb = new();
        ProcessExpression processExpression = (ex) =>
        {
            sb.Append(((ConstantExpression)ex).Value);
        };

        // Act
        JsBinaryExpressionBuilder.AppendEquality(sb, expression, processExpression);

        // Assert
        Assert.Equal("(10)!==(20)", sb.ToString());
    }

    [Fact]
    public void AppendEquality_WithAddExpression_AppendsExpression()
    {
        // Arrange
        var expression = Expression.Add(Expression.Constant(10), Expression.Constant(20));
        StringBuilder sb = new();
        ProcessExpression processExpression = (ex) =>
        {
            sb.Append(((ConstantExpression)ex).Value);
        };

        // Act
        JsBinaryExpressionBuilder.AppendEquality(sb, expression, processExpression);

        // Assert
        Assert.Equal("(10)+(20)", sb.ToString());
    }

    [Fact]
    public void AppendEquality_WithSubtractExpression_AppendsExpression()
    {
        // Arrange
        var expression = Expression.Subtract(Expression.Constant(10), Expression.Constant(20));
        StringBuilder sb = new();
        ProcessExpression processExpression = (ex) =>
        {
            sb.Append(((ConstantExpression)ex).Value);
        };

        // Act
        JsBinaryExpressionBuilder.AppendEquality(sb, expression, processExpression);

        // Assert
        Assert.Equal("(10)-(20)", sb.ToString());
    }

    [Fact]
    public void AppendEquality_WithMultiplyExpression_AppendsExpression()
    {
        // Arrange
        var expression = Expression.Multiply(Expression.Constant(10), Expression.Constant(20));
        StringBuilder sb = new();
        ProcessExpression processExpression = (ex) =>
        {
            sb.Append(((ConstantExpression)ex).Value);
        };

        // Act
        JsBinaryExpressionBuilder.AppendEquality(sb, expression, processExpression);

        // Assert
        Assert.Equal("(10)*(20)", sb.ToString());
    }

    [Fact]
    public void AppendEquality_WithDivideExpression_AppendsExpression()
    {
        // Arrange
        var expression = Expression.Divide(Expression.Constant(10), Expression.Constant(20));
        StringBuilder sb = new();
        ProcessExpression processExpression = (ex) =>
        {
            sb.Append(((ConstantExpression)ex).Value);
        };

        // Act
        JsBinaryExpressionBuilder.AppendEquality(sb, expression, processExpression);

        // Assert
        Assert.Equal("(10)/(20)", sb.ToString());
    }

    [Fact]
    public void AppendEquality_WithModuloExpression_AppendsExpression()
    {
        // Arrange
        var expression = Expression.Modulo(Expression.Constant(10), Expression.Constant(20));
        StringBuilder sb = new();
        ProcessExpression processExpression = (ex) =>
        {
            sb.Append(((ConstantExpression)ex).Value);
        };

        // Act
        JsBinaryExpressionBuilder.AppendEquality(sb, expression, processExpression);

        // Assert
        Assert.Equal("(10)%(20)", sb.ToString());
    }

    [Fact]
    public void AppendEquality_WithGreaterThanExpression_AppendsExpression()
    {
        // Arrange
        var expression = Expression.GreaterThan(Expression.Constant(10), Expression.Constant(20));
        StringBuilder sb = new();
        ProcessExpression processExpression = (ex) =>
        {
            sb.Append(((ConstantExpression)ex).Value);
        };

        // Act
        JsBinaryExpressionBuilder.AppendEquality(sb, expression, processExpression);

        // Assert
        Assert.Equal("(10)>(20)", sb.ToString());
    }

    [Fact]
    public void AppendEquality_WithLessThanExpression_AppendsExpression()
    {
        // Arrange
        var expression = Expression.LessThan(Expression.Constant(10), Expression.Constant(20));
        StringBuilder sb = new();
        ProcessExpression processExpression = (ex) =>
        {
            sb.Append(((ConstantExpression)ex).Value);
        };

        // Act
        JsBinaryExpressionBuilder.AppendEquality(sb, expression, processExpression);

        // Assert
        Assert.Equal("(10)<(20)", sb.ToString());
    }

    [Fact]
    public void AppendEquality_WithAndAlsoExpression_AppendsExpression()
    {
        // Arrange
        var expression = Expression.AndAlso(Expression.Constant(true), Expression.Constant(false));
        StringBuilder sb = new();
        ProcessExpression processExpression = (ex) =>
        {
            sb.Append(((ConstantExpression)ex).Value);
        };

        // Act
        JsBinaryExpressionBuilder.AppendEquality(sb, expression, processExpression);

        // Assert
        Assert.Equal("(True)&&(False)", sb.ToString());
    }

    [Fact]
    public void AppendEquality_WithOrElseExpression_AppendsExpression()
    {
        // Arrange
        var expression = Expression.OrElse(Expression.Constant(true), Expression.Constant(false));
        StringBuilder sb = new();
        ProcessExpression processExpression = (ex) =>
        {
            sb.Append(((ConstantExpression)ex).Value);
        };

        // Act
        JsBinaryExpressionBuilder.AppendEquality(sb, expression, processExpression);

        // Assert
        Assert.Equal("(True)||(False)", sb.ToString());
    }

    [Fact]
    public void AppendEquality_WithGreaterThanOrEqualExpression_AppendsExpression()
    {
        // Arrange
        var expression = Expression.GreaterThanOrEqual(Expression.Constant(10), Expression.Constant(20));
        StringBuilder sb = new();
        ProcessExpression processExpression = (ex) =>
        {
            sb.Append(((ConstantExpression)ex).Value);
        };

        // Act
        JsBinaryExpressionBuilder.AppendEquality(sb, expression, processExpression);

        // Assert
        Assert.Equal("(10)>=(20)", sb.ToString());
    }

    [Fact]
    public void AppendEquality_WithLessThanOrEqualExpression_AppendsExpression()
    {
        // Arrange
        var expression = Expression.LessThanOrEqual(Expression.Constant(10), Expression.Constant(20));
        StringBuilder sb = new();
        ProcessExpression processExpression = (ex) =>
        {
            sb.Append(((ConstantExpression)ex).Value);
        };

        // Act
        JsBinaryExpressionBuilder.AppendEquality(sb, expression, processExpression);

        // Assert
        Assert.Equal("(10)<=(20)", sb.ToString());
    }
}