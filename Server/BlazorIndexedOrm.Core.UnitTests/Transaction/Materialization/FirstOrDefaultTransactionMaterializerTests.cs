using System.Linq.Expressions;
using BlazorIndexedOrm.Core.Transaction.JsExpression;
using BlazorIndexedOrm.Core.Transaction.JsExpression.BinaryTranslation;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MemberTranslation;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation;
using Microsoft.JSInterop;
using Moq;

namespace BlazorIndexedOrm.Core.Transaction.Materialization;

public class FirstOrDefaultTransactionMaterializerTests
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
        var objectStore = "objectStore";
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        // Act
        Action action = () => new FirstOrDefaultTransactionMaterializer<object>(jsRuntime, jsExpressionBuilder, conditions, database, objectStore);
        
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
        var objectStore = "objectStore";
        var methodCallTranslatorFactory = new Mock<IMethodCallTranslatorFactory>();
        var memberCallTranslatorFactory = new Mock<IMemberTranslatorFactory>();
        var binaryTranslatorFactory = new Mock<IBinaryTranslatorFactory>();
        var jsExpressionBuilder = new JsExpressionBuilder(methodCallTranslatorFactory.Object, memberCallTranslatorFactory.Object, binaryTranslatorFactory.Object);

        // Act
        Action action = () => new FirstOrDefaultTransactionMaterializer<object>(jsRuntime, jsExpressionBuilder, conditions, database, objectStore);

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
        var objectStore = "objectStore";
        var methodCallTranslatorFactory = new Mock<IMethodCallTranslatorFactory>();
        var memberCallTranslatorFactory = new Mock<IMemberTranslatorFactory>();
        var binaryTranslatorFactory = new Mock<IBinaryTranslatorFactory>();
        var jsExpressionBuilder = new JsExpressionBuilder(methodCallTranslatorFactory.Object, memberCallTranslatorFactory.Object, binaryTranslatorFactory.Object);

        // Act
        Action action = () => new FirstOrDefaultTransactionMaterializer<object>(jsRuntime, jsExpressionBuilder, conditions, database, objectStore);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("database", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenObjectStoreIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var conditions = new TransactionConditions<object>();
        var mockJsRuntime = new Mock<IJSRuntime>().Object;
        var factory = new Mock<IIndexedDbTransactionProviderFactory>();
        factory.SetupGet(x => x.JsRuntime).Returns(mockJsRuntime);
        var database = new Mock<IndexedDbDatabase>(factory.Object).Object;
        string objectStore = null!;
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;

        // Act
        Action action = () => new FirstOrDefaultTransactionMaterializer<object>(mockJsRuntime, jsExpressionBuilder, conditions, database, objectStore);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("objectStore", exception.ParamName);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCalled_CallsJsRuntimeInvokeAsync()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>();
        jsRuntime.Setup(e => e.InvokeAsync<object>(JsMethodNameConstants.GetVersion, It.IsAny<CancellationToken>(), It.IsAny<object[]>())).ReturnsAsync(1ul);
        var factory = new Mock<IIndexedDbTransactionProviderFactory>();
        factory.SetupGet(x => x.JsRuntime).Returns(jsRuntime.Object);
        var database = new Mock<IndexedDbDatabase>(factory.Object).Object;
        var objectStore = "objectStore";
        var result = new object();
        jsRuntime.Setup(x => x.InvokeAsync<object>(JsMethodNameConstants.FirstOrDefault, It.IsAny<CancellationToken>(), It.IsAny<object[]>())).ReturnsAsync(result);
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        var materializer = new FirstOrDefaultTransactionMaterializer<object>(jsRuntime.Object, jsExpressionBuilder, new TransactionConditions<object>(), database, objectStore);

        // Act
        var actual = await materializer.ExecuteAsync();
        
        // Assert
        Assert.Equal(result, actual);
    }

    [Fact]
    public async Task ExecuteAsync_WhenTransactionHasConditions_PassesFunctionArray()
    {
        // Arrange
        string expressionResult = "expression";
        string[] expected = new[] { expressionResult, expressionResult };
        string[]? result = null;
        var jsRuntime = new Mock<IJSRuntime>();
        jsRuntime.Setup(e => e.InvokeAsync<object>(JsMethodNameConstants.GetVersion, It.IsAny<CancellationToken>(), It.IsAny<object[]>())).ReturnsAsync(It.IsAny<ulong>());
        Delegate callBack = (string _, CancellationToken _, params object[] parameters) =>
        {
            result = (string[])parameters[3];
        };
        
        jsRuntime.Setup(x => x.InvokeAsync<object>(JsMethodNameConstants.FirstOrDefault, It.IsAny<CancellationToken>(), It.IsAny<object[]>()))
            .Callback(callBack).ReturnsAsync(It.IsAny<object>());
        
        var factory = new Mock<IIndexedDbTransactionProviderFactory>();
        factory.SetupGet(x => x.JsRuntime).Returns(jsRuntime.Object);
        var database = new Mock<IndexedDbDatabase>(factory.Object).Object;
        var mockExpressionBuilder = new Mock<IExpressionBuilder>();
        mockExpressionBuilder.Setup(e => e.ProcessExpression(It.IsAny<LambdaExpression>())).Returns(expressionResult);
        
        var conditions = new TransactionConditions<object>();
        conditions.AddCondition(_ => true);
        conditions.AddCondition(_ => false);
        var materializer = new FirstOrDefaultTransactionMaterializer<object>(jsRuntime.Object, mockExpressionBuilder.Object, conditions, database, "objectStore");

        // Act
        await materializer.ExecuteAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected, result);
    }
}