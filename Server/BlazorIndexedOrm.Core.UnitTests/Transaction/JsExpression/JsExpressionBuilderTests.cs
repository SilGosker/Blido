using System.Linq.Expressions;
using System.Reflection;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation;
using BlazorIndexedOrm.Core.UnitTests.Mock.IndexedDbDatabase;
using Moq;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression;

public class JsExpressionBuilderTests
{
    [Fact]
    public void Constructor_WithNull_ThrowsArgumentNullException()
    {
        // Arrange
        IJsMethodCallTranslatorFactory methodCallTranslatorFactory = null!;

        // Act
        var act = () => new JsExpressionBuilder(methodCallTranslatorFactory);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("methodCallTranslatorFactory", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithFactory_ReturnsObject()
    {
        // Arrange
        Mock<IJsMethodCallTranslatorFactory> methodCallTranslatorFactoryMock = new();

        // Act
        var expressionBuilder = new JsExpressionBuilder(methodCallTranslatorFactoryMock.Object);

        // Assert
        Assert.NotNull(expressionBuilder.Result);
    }

}