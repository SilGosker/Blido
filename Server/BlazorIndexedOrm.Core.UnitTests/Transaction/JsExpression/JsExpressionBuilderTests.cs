using System.Linq.Expressions;
using System.Text;
using BlazorIndexedOrm.Core.Transaction.JsExpression.BinaryTranslation;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MemberTranslation;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation;
using BlazorIndexedOrm.Core.Transaction.JsExpression.UnaryTranslation;
using Moq;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression;

public class JsExpressionBuilderTests
{
    [Fact]
    public void Constructor_WithNullMethodCallTranslatorFactory_ThrowsArgumentNullException()
    {
        // Arrange
        IMethodCallTranslatorFactory methodCallTranslatorFactory = null!;
        IMemberTranslatorFactory memberTranslatorFactory = new Mock<IMemberTranslatorFactory>().Object;
        IBinaryTranslatorFactory binaryTranslatorFactory = new Mock<IBinaryTranslatorFactory>().Object;
        IUnaryTranslatorFactory unaryTranslatorFactory = new Mock<IUnaryTranslatorFactory>().Object;

        // Act
        var act = () => new JsExpressionBuilder(methodCallTranslatorFactory, memberTranslatorFactory, binaryTranslatorFactory, unaryTranslatorFactory);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("methodCallTranslatorFactory", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullMemberTranslatorFactory_ThrowsArgumentNullException()
    {
        // Arrange
        var methodCallTranslatorFactory = new Mock<IMethodCallTranslatorFactory>().Object;
        IMemberTranslatorFactory memberTranslatorFactory = null!;
        IBinaryTranslatorFactory binaryTranslatorFactory = new Mock<IBinaryTranslatorFactory>().Object;
        IUnaryTranslatorFactory unaryTranslatorFactory = new Mock<IUnaryTranslatorFactory>().Object;

        // Act
        var act = () => new JsExpressionBuilder(methodCallTranslatorFactory, memberTranslatorFactory, binaryTranslatorFactory, unaryTranslatorFactory);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("memberTranslatorFactory", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullBinaryTranslatorFactory_ThrowsArgumentNullException()
    {
        // Arrange
        var methodCallTranslatorFactory = new Mock<IMethodCallTranslatorFactory>().Object;
        var memberTranslatorFactory = new Mock<IMemberTranslatorFactory>().Object;
        IBinaryTranslatorFactory binaryTranslatorFactory = null!;
        IUnaryTranslatorFactory unaryTranslatorFactory = new Mock<IUnaryTranslatorFactory>().Object;

        // Act
        var act = () => new JsExpressionBuilder(methodCallTranslatorFactory, memberTranslatorFactory, binaryTranslatorFactory, unaryTranslatorFactory);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("binaryTranslatorFactory", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullUnaryTranslatorFactory_ThrowsArgumentNullException()
    {
        // Arrange
        var methodCallTranslatorFactory = new Mock<IMethodCallTranslatorFactory>().Object;
        var memberTranslatorFactory = new Mock<IMemberTranslatorFactory>().Object;
        var binaryTranslatorFactory = new Mock<IBinaryTranslatorFactory>().Object;
        IUnaryTranslatorFactory unaryTranslatorFactory = null!;

        // Act
        var act = () => new JsExpressionBuilder(methodCallTranslatorFactory, memberTranslatorFactory, binaryTranslatorFactory, unaryTranslatorFactory);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("unaryTranslatorFactory", exception.ParamName);
    }

    [Fact]
    public void ProcessExpression_WithNull_ThrowsArgumentNullException()
    {
        // Arrange
        var methodCallTranslatorFactory = new Mock<IMethodCallTranslatorFactory>().Object;
        var memberTranslatorFactory = new Mock<IMemberTranslatorFactory>().Object;
        var binaryTranslatorFactory = new Mock<IBinaryTranslatorFactory>().Object;
        var unaryTranslatorFactory = new Mock<IUnaryTranslatorFactory>().Object;
        var jsExpressionBuilder = new JsExpressionBuilder(methodCallTranslatorFactory, memberTranslatorFactory, binaryTranslatorFactory, unaryTranslatorFactory);
        LambdaExpression expression = null!;

        // Act
        var act = () => jsExpressionBuilder.ProcessExpression(expression);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("expression", exception.ParamName);
    }

    [Fact]
    public void ProcessExpression_WithConstantExpression_CallsJsConstantExpressionBuilderAppendConstant()
    {
        // Arrange
        var methodCallTranslatorFactory = new Mock<IMethodCallTranslatorFactory>().Object;
        var memberTranslatorFactory = new Mock<IMemberTranslatorFactory>().Object;
        var binaryTranslatorFactory = new Mock<IBinaryTranslatorFactory>().Object;
        var unaryTranslatorFactory = new Mock<IUnaryTranslatorFactory>().Object;
        var jsExpressionBuilder = new JsExpressionBuilder(methodCallTranslatorFactory, memberTranslatorFactory, binaryTranslatorFactory, unaryTranslatorFactory);
        var expression = Expression.Constant(1);

        // Act
        string s = jsExpressionBuilder.ProcessExpression(Expression.Lambda(expression));
        
        // Assert
        Assert.Equal("()=>1", s);
    }

    [Fact]
    public void ProcessExpression_WithMemberExpression_CallsJsMemberExpressionBuilderAppendMember()
    {
        // Arrange
        var methodCallTranslatorFactory = new Mock<IMethodCallTranslatorFactory>().Object;
        var memberTranslatorFactory = new JsMemberTranslatorFactory();
        var binaryTranslatorFactory = new Mock<IBinaryTranslatorFactory>().Object;
        var unaryTranslatorFactory = new Mock<IUnaryTranslatorFactory>().Object;
        var jsExpressionBuilder = new JsExpressionBuilder(methodCallTranslatorFactory, memberTranslatorFactory, binaryTranslatorFactory, unaryTranslatorFactory);
        var expression = Expression.Property(Expression.Constant("test"), "Length");
        
        // Act
        string s = jsExpressionBuilder.ProcessExpression(Expression.Lambda(expression));
        
        // Assert
        Assert.Equal("()=>\"test\".length", s);
    }

    [Fact]
    public void ProcessExpression_WithUnaryExpression_CallsJsUnaryExpressionBuilderAppendUnary()
    {
        // Arrange
        var methodCallTranslatorFactory = new Mock<IMethodCallTranslatorFactory>().Object;
        var memberTranslatorFactory = new Mock<IMemberTranslatorFactory>().Object;
        var binaryTranslatorFactory = new Mock<IBinaryTranslatorFactory>().Object;
        var unaryTranslatorFactory = new JsUnaryTranslatorFactory();
        var jsExpressionBuilder = new JsExpressionBuilder(methodCallTranslatorFactory, memberTranslatorFactory, binaryTranslatorFactory, unaryTranslatorFactory );
        var expression = Expression.Not(Expression.Constant(true));

        // Act
        string s = jsExpressionBuilder.ProcessExpression(Expression.Lambda(expression));

        // Assert
        Assert.Equal("()=>(!true)", s);
    }

    [Fact]
    public void ProcessExpression_WithLambdaExpression_CallsJsLambdaExpressionBuilderAppendLambda()
    {
        // Arrange
        var methodCallTranslatorFactory = new Mock<IMethodCallTranslatorFactory>().Object;
        var memberTranslatorFactory = new Mock<IMemberTranslatorFactory>().Object;
        var binaryTranslatorFactory = new Mock<IBinaryTranslatorFactory>().Object;
        var unaryTranslatorFactory = new Mock<IUnaryTranslatorFactory>().Object;
        var jsExpressionBuilder = new JsExpressionBuilder(methodCallTranslatorFactory, memberTranslatorFactory, binaryTranslatorFactory, unaryTranslatorFactory);
        var expression = Expression.Lambda(Expression.Lambda(Expression.Constant(1)));
        // Act
        string s = jsExpressionBuilder.ProcessExpression(expression);
        // Assert
        Assert.Equal("()=>()=>1", s);
    }

    [Fact]
    public void ProcessExpression_WithMethodCallExpression_CallsJsMethodExpressionBuilderAppendMethod()
    {
        // Arrange
        var methodCallTranslatorFactory = new JsMethodCallTranslatorFactory();
        var memberTranslatorFactory = new Mock<IMemberTranslatorFactory>().Object;
        var binaryTranslatorFactory = new Mock<IBinaryTranslatorFactory>().Object;
        var unaryTranslatorFactory = new Mock<IUnaryTranslatorFactory>().Object;
        var jsExpressionBuilder = new JsExpressionBuilder(methodCallTranslatorFactory, memberTranslatorFactory, binaryTranslatorFactory, unaryTranslatorFactory);
        var method = typeof(string).GetMethod(nameof(string.Substring), new[] { typeof(int), typeof(int) })!;
        var expression = Expression.Call(Expression.Constant("test"), method, Expression.Constant(1), Expression.Constant(2));
        
        // Act
        string s = jsExpressionBuilder.ProcessExpression(Expression.Lambda(expression));
        
        // Assert
        Assert.Equal("()=>\"test\".substring(1,2)", s);
    }

    [Fact]
    public void ProcessExpression_WithParameterExpression_CallsJsParameterExpressionBuilderAppendParameter()
    {
        // Arrange
        var methodCallTranslatorFactory = new Mock<IMethodCallTranslatorFactory>().Object;
        var memberTranslatorFactory = new Mock<IMemberTranslatorFactory>().Object;
        var binaryTranslatorFactory = new Mock<IBinaryTranslatorFactory>().Object;
        var unaryTranslatorFactory = new Mock<IUnaryTranslatorFactory>().Object;
        var jsExpressionBuilder = new JsExpressionBuilder(methodCallTranslatorFactory, memberTranslatorFactory, binaryTranslatorFactory, unaryTranslatorFactory);
        var expression = Expression.Parameter(typeof(int), "x");
        var result = Expression.Constant("test");

        // Act
        string s = jsExpressionBuilder.ProcessExpression(Expression.Lambda(result, expression));
        
        // Assert
        Assert.Equal("(x)=>\"test\"", s);
    }
}