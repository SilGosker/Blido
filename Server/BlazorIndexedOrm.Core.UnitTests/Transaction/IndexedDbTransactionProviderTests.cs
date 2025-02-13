using System.Linq.Expressions;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MemberTranslation;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation;
using BlazorIndexedOrm.Core.Transaction.JsExpression;
using Microsoft.JSInterop;
using Moq;

namespace BlazorIndexedOrm.Core.Transaction;

public class IndexedDbTransactionProviderTests
{
    [Fact]
    public void Constructor_WhenJsRuntimeIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var mockJsRuntime = new Mock<IJSRuntime>();
        var mockFactory = new Mock<IIndexedDbTransactionProviderFactory>();
        mockFactory.SetupGet(x => x.JsRuntime).Returns(mockJsRuntime.Object);
        var indexedDbDatabase = new Mock<IndexedDbDatabase>(mockFactory.Object).Object;
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        IJSRuntime jsRuntime = null!;

        // Act
        var act = () => new IndexedDbTransactionProvider<object>(jsRuntime, indexedDbDatabase, jsExpressionBuilder);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("jsRuntime", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenIndexedDbDatabaseIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        IndexedDbDatabase indexedDbDatabase = null!;
        var methodCallTranslatorFactory = new Mock<IMethodCallTranslatorFactory>();
        var memberCallTranslatorFactory = new Mock<IMemberTranslatorFactory>();
        var jsExpressionBuilder = new JsExpressionBuilder(methodCallTranslatorFactory.Object, memberCallTranslatorFactory.Object);

        // Act
        var act = () => new IndexedDbTransactionProvider<object>(jsRuntime, indexedDbDatabase, jsExpressionBuilder);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("database", exception.ParamName);
    }

    [Fact]
    public async Task Execute_WhenMethodNameIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var factory = new Mock<IIndexedDbTransactionProviderFactory>();
        factory.SetupGet(x => x.JsRuntime).Returns(jsRuntime);
        var indexedDbDatabase = new Mock<IndexedDbDatabase>(factory.Object).Object;
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        var transactionProvider = new IndexedDbTransactionProvider<object>(jsRuntime, indexedDbDatabase, jsExpressionBuilder);

        // Act
        var act = () => transactionProvider.Execute<object>(null!);

        // Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(act);
        Assert.Equal("methodName", exception.ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public async Task Execute_WhenMethodNameIsEmpty_ThrowsArgumentException(string methodName)
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var factory = new Mock<IIndexedDbTransactionProviderFactory>();
        factory.SetupGet(x => x.JsRuntime).Returns(jsRuntime);
        var indexedDbDatabase = new Mock<IndexedDbDatabase>(factory.Object).Object;
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        var transactionProvider = new IndexedDbTransactionProvider<object>(jsRuntime, indexedDbDatabase, jsExpressionBuilder);

        // Act
        var act = () => transactionProvider.Execute<object>(methodName);

        // Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(act);
        Assert.Equal("methodName", exception.ParamName);
    }

    [Fact]
    public void Where_WhenExpressionIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var factory = new Mock<IIndexedDbTransactionProviderFactory>();
        factory.SetupGet(x => x.JsRuntime).Returns(jsRuntime);
        var indexedDbDatabase = new Mock<IndexedDbDatabase>(factory.Object).Object;
        var jsExpressionBuilder = new Mock<IExpressionBuilder>().Object;
        var transactionProvider = new IndexedDbTransactionProvider<object>(jsRuntime, indexedDbDatabase, jsExpressionBuilder);
        
        // Act
        var act = () => transactionProvider.Where(null!);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("expression", exception.ParamName);
    }

    [Fact]
    public void Where_WhenExpressionIsNotNull_ReturnsInstance()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var factory = new Mock<IIndexedDbTransactionProviderFactory>();
        factory.SetupGet(x => x.JsRuntime).Returns(jsRuntime);
        var indexedDbDatabase = new Mock<IndexedDbDatabase>(factory.Object).Object;
        var methodCallTranslatorFactory = new Mock<IMethodCallTranslatorFactory>();
        var memberCallTranslatorFactory = new Mock<IMemberTranslatorFactory>();
        var jsExpressionBuilder = new JsExpressionBuilder(methodCallTranslatorFactory.Object, memberCallTranslatorFactory.Object);
        var transactionProvider = new IndexedDbTransactionProvider<object>(jsRuntime, indexedDbDatabase, jsExpressionBuilder);
        Expression<Func<object, bool>> expression = _ => true;

        // Act
        var result = transactionProvider.Where(expression);

        // Assert
        Assert.Same(transactionProvider, result);
    }
}