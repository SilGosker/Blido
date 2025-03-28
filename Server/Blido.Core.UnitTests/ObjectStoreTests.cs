﻿using System.Linq.Expressions;
using Blido.Core.Transaction;
using Microsoft.JSInterop;
using Moq;

namespace Blido.Core;

public class ObjectStoreTests
{
    [Fact]
    public void Constructor_WithNullProvider_ThrowsArgumentNullException()
    {
        // Arrange
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        var jsRuntime = new Mock<IJSRuntime>().Object;
        objectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntime);
        ITransactionProvider<object> provider = null!;
        IndexedDbDatabase database = new(new MockIndexedDbDatabase(objectStoreFactory.Object), jsRuntime);

        // Act
        Action act = () => new ObjectStore<object>(database, provider);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("provider", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullDatabase_ThrowsArgumentNullException()
    {
        // Arrange
        var provider = new Mock<ITransactionProvider<object>>().Object;
        IndexedDbDatabase database = null!;

        // Act
        Action act = () => new ObjectStore<object>(database, provider);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("database", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithValidProvider_SetsName()
    {
        // Arrange
        var provider = new Mock<ITransactionProvider<object>>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        var jsRuntime = new Mock<IJSRuntime>().Object;
        objectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntime);
        IndexedDbDatabase database = new(new MockIndexedDbDatabase(objectStoreFactory.Object), jsRuntime);

        // Act
        var objectStoreSet = new ObjectStore<object>(database, provider);

        // Assert
        Assert.Equal("Object", objectStoreSet.Name);
    }

    [Fact]
    public void Where_WithNull_ThrowsArgumentNullException()
    {
        // Arrange
        var provider = new Mock<ITransactionProvider<object>>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        var jsRuntime = new Mock<IJSRuntime>().Object;
        objectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntime);
        IndexedDbDatabase database = new(new MockIndexedDbDatabase(objectStoreFactory.Object), jsRuntime);
        var objectStoreSet = new ObjectStore<object>(database, provider);

        // Act
        Action act = () => objectStoreSet.Where(null!);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("expression", exception.ParamName);
    }

    [Fact]
    public void Where_WithExpression_CallsWhereOnTransactionProvider()
    {
        // Arrange
        var providerMock = new Mock<ITransactionProvider<object>>();
        var provider = providerMock.Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        var jsRuntime = new Mock<IJSRuntime>().Object;
        objectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntime);
        IndexedDbDatabase database = new(new MockIndexedDbDatabase(objectStoreFactory.Object), jsRuntime);
        var objectStoreSet = new ObjectStore<object>(database, provider);
        Expression<Func<object, bool>> expression = _ => true;

        // Act
        objectStoreSet.Where(expression);

        // Assert
        providerMock.Verify(x => x.Where(expression), Times.Once);
    }

    [Fact]
    public void Where_WithExpression_ReturnsInstance()
    {
        // Arrange
        var provider = new Mock<ITransactionProvider<object>>().Object;
        var objectStoreFactory = new Mock<IObjectStoreFactory>();
        var jsRuntime = new Mock<IJSRuntime>().Object;
        objectStoreFactory.SetupGet(x => x.JsRuntime).Returns(jsRuntime);
        IndexedDbDatabase database = new(new MockIndexedDbDatabase(objectStoreFactory.Object), jsRuntime);
        var objectStoreSet = new ObjectStore<object>(database, provider);
        Expression<Func<object, bool>> expression = _ => true;

        // Act
        var result = objectStoreSet.Where(expression);
        
        // Assert
        Assert.Same(objectStoreSet, result);
    }
}