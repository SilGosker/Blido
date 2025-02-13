using System.Linq.Expressions;
using System.Reflection;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MemberTranslation;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation;
using BlazorIndexedOrm.Core.UnitTests.Mock.IndexedDbDatabase;
using Moq;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression;
// TODO: proper testing
public class JsExpressionBuilderTests
{
    [Fact]
    public void Constructor_WithNull_ThrowsArgumentNullException()
    {
        // Arrange
        IMethodCallTranslatorFactory methodCallTranslatorFactory = null!;
        IMemberTranslatorFactory memberTranslatorFactory = null!;

        // Act
        var act = () => new JsExpressionBuilder(methodCallTranslatorFactory, memberTranslatorFactory);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("methodCallTranslatorFactory", exception.ParamName);
    }

}