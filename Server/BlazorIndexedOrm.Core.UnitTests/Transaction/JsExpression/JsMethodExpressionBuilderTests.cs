using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation;
using Moq;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression;

public class JsMethodExpressionBuilderTests
{
    [Fact]
    public void AppendMethod_WhenMethodNotSupported_ThrowsException()
    {
        // Arrange
        var builder = new StringBuilder();
        var translatorFactory = new Mock<IJsMethodCallTranslatorFactory>();
        var methodCall = Expression.Call(Expression.Constant("test"), typeof(string).GetMethod(nameof(string.ToUpper), Array.Empty<Type>())!);
        ProcessExpression processExpression = (_ => { });

        translatorFactory.Setup(x => x.TryGetValue(It.IsAny<MethodInfo>(), out It.Ref<TranslateMethodCall>.IsAny))
            .Returns(false);

        // Act
        Action act = () => JsMethodExpressionBuilder.AppendMethod(builder, translatorFactory.Object, methodCall, processExpression);

        // Assert
        Assert.Throws<NotSupportedException>(act);
    }
    
    [Fact]
    public void AppendMethod_WhenMethodSupported_AppendsMethod()
    {
        // Arrange
        var builder = new StringBuilder();
        var translatorFactory = new Mock<IJsMethodCallTranslatorFactory>();
        var methodCall = Expression.Call(Expression.Constant("test"), typeof(string).GetMethod(nameof(string.ToUpper), Array.Empty<Type>())!);
        ProcessExpression processExpression = (_ => { });
        TranslateMethodCall translateMethodCall = (sb, _, _) =>
        {
            sb.Append("test.toUpperCase()");
        };

        translatorFactory.Setup(x => x.TryGetValue(It.IsAny<MethodInfo>(), out translateMethodCall))
            .Returns(true);

        // Act
        JsMethodExpressionBuilder.AppendMethod(builder, translatorFactory.Object, methodCall, processExpression);

        // Assert
        Assert.Equal("test.toUpperCase()", builder.ToString());
    }
}