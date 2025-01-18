using System.Linq.Expressions;
using BlazorIndexedOrm.Core.Transaction;
using Microsoft.JSInterop;
using Moq;

namespace BlazorIndexedOrm.Core.UnitTests.Transaction;

public class IndexedDbTransactionProviderTests
{
    [Fact]
    public void Constructor_WhenJsRuntimeIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IJSRuntime jsRuntime = null!;
        var mockRuntime = new Mock<IJSRuntime>().Object;
        var indexedDbDatabase = new Mock<IndexedDbDatabase>(mockRuntime).Object;

        // Act
        var act = () => new IndexedDbTransactionProvider<object>(jsRuntime, indexedDbDatabase);

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

        // Act
        var act = () => new IndexedDbTransactionProvider<object>(jsRuntime, indexedDbDatabase);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("database", exception.ParamName);
    }

    [Fact]
    public async Task Execute_WhenMethodNameIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var jsRuntime = new Mock<IJSRuntime>().Object;
        var indexedDbDatabase = new Mock<IndexedDbDatabase>(jsRuntime).Object;
        var transactionProvider = new IndexedDbTransactionProvider<object>(jsRuntime, indexedDbDatabase);

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
        var indexedDbDatabase = new Mock<IndexedDbDatabase>(jsRuntime).Object;
        var transactionProvider = new IndexedDbTransactionProvider<object>(jsRuntime, indexedDbDatabase);

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
        var indexedDbDatabase = new Mock<IndexedDbDatabase>(jsRuntime).Object;
        var transactionProvider = new IndexedDbTransactionProvider<object>(jsRuntime, indexedDbDatabase);
        
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
        var indexedDbDatabase = new Mock<IndexedDbDatabase>(jsRuntime).Object;
        var transactionProvider = new IndexedDbTransactionProvider<object>(jsRuntime, indexedDbDatabase);
        Expression<Func<object, bool>> expression = _ => true;

        // Act
        var result = transactionProvider.Where(expression);

        // Assert
        Assert.Same(transactionProvider, result);
    }
}