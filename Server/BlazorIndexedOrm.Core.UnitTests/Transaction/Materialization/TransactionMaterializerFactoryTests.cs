using BlazorIndexedOrm.Core.Transaction.JsExpression;
using BlazorIndexedOrm.Core.Transaction.JsExpression.BinaryTranslation;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MemberTranslation;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation;
using Microsoft.JSInterop;
using Moq;

namespace BlazorIndexedOrm.Core.Transaction.Materialization;

public class TransactionMaterializerFactoryTests
{
    [Fact]
    public void Constructor_WhenJsRuntimeIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IJSRuntime jsRuntime = null!;
        var conditions = new TransactionConditions<object>();
        var mockJsRuntime = new Mock<IJSRuntime>().Object;
        var factory = new Mock<IIndexedDbTransactionProviderFactory>();
        factory.SetupGet(x => x.JsRuntime).Returns(mockJsRuntime);
        var database = new Mock<IndexedDbDatabase>(factory.Object).Object;
        var memberTranslatorFactoryMock = new Mock<IMemberTranslatorFactory>();
        var methodCallTranslatorFactoryMock = new Mock<IMethodCallTranslatorFactory>();
        var binaryTranslatorFactory = new Mock<IBinaryTranslatorFactory>();
        var expressionBuilder = new JsExpressionBuilder(methodCallTranslatorFactoryMock.Object, memberTranslatorFactoryMock.Object, binaryTranslatorFactory.Object);

        // Act
        Action action = () => new TransactionMaterializerFactory<object>(jsRuntime, conditions, database, expressionBuilder);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("jsRuntime", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenConditionsIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        TransactionConditions<object> conditions = null!;
        var factory = new Mock<IIndexedDbTransactionProviderFactory>();
        factory.SetupGet(x => x.JsRuntime).Returns(jsRuntime);
        var database = new Mock<IndexedDbDatabase>(factory.Object).Object;
        var memberTranslatorFactoryMock = new Mock<IMemberTranslatorFactory>();
        var methodCallTranslatorFactoryMock = new Mock<IMethodCallTranslatorFactory>();
        var binaryTranslatorFactory = new Mock<IBinaryTranslatorFactory>();
        var expressionBuilder = new JsExpressionBuilder(methodCallTranslatorFactoryMock.Object, memberTranslatorFactoryMock.Object, binaryTranslatorFactory.Object);

        // Act
        Action action = () => new TransactionMaterializerFactory<object>(jsRuntime, conditions, database, expressionBuilder);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("conditions", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenDatabaseIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var conditions = new TransactionConditions<object>();
        IndexedDbDatabase database = null!;
        var memberTranslatorFactoryMock = new Mock<IMemberTranslatorFactory>();
        var methodCallTranslatorFactoryMock = new Mock<IMethodCallTranslatorFactory>();
        var binaryTranslatorFactory = new Mock<IBinaryTranslatorFactory>();
        var expressionBuilder = new JsExpressionBuilder(methodCallTranslatorFactoryMock.Object, memberTranslatorFactoryMock.Object, binaryTranslatorFactory.Object);

        // Act
        Action action = () => new TransactionMaterializerFactory<object>(jsRuntime, conditions, database, expressionBuilder);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("database", exception.ParamName);
    }
}