using System.Linq.Expressions;
using System.Text;
using Blido.Core.Transaction.JsExpression.BinaryTranslation;
using Moq;

namespace Blido.Core.Transaction.JsExpression;

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
    public void AppendBinary_WhenBinaryTranslatorFactoryContainsBinary_TranslatesBinary()
    {
        // Arrange
        var binary = Expression.Add(Expression.Constant(1), Expression.Constant(2));
        var binaryTranslatorFactory = new Mock<IBinaryTranslatorFactory>();
        ProcessExpression processExpression = (_) => { };
        TranslateBinary? translateBinary = (_, _, _) =>
        {

        };
        var sb = new StringBuilder();
        binaryTranslatorFactory.Setup(x => x.TryGetValue(binary, out translateBinary)).Returns(true);

        // Act
        JsBinaryExpressionBuilder.AppendBinary(sb, binaryTranslatorFactory.Object, binary, processExpression);
        
        // Assert
        binaryTranslatorFactory.Verify(x => x.TryGetValue(binary, out translateBinary), Times.Once);
    }

    [Fact]
    public void AppendBinary_WhenBinaryTranslatorFactoryDoesNotContainBinary_ThrowsNotSupportedException()
    {
        // Arrange
        var binary = Expression.Add(Expression.Constant(1), Expression.Constant(2));
        var binaryTranslatorFactory = new Mock<IBinaryTranslatorFactory>();
        ProcessExpression processExpression = (_) => { };
        var sb = new StringBuilder();
        binaryTranslatorFactory.Setup(x => x.TryGetValue(binary, out It.Ref<TranslateBinary>.IsAny!)).Returns(false);
        
        // Act
        Action act = () => JsBinaryExpressionBuilder.AppendBinary(sb, binaryTranslatorFactory.Object, binary, processExpression);
        
        // Assert
        Assert.Throws<NotSupportedException>(act);
    }

}