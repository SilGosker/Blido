using System.Linq.Expressions;
using Blido.Core.Transaction;
using Moq;

namespace Blido.Core;

public class ObjectStoreTests
{
    [Fact]
    public void Constructor_WithNullProvider_ThrowsArgumentNullException()
    {
        // Arrange
        ITransactionProvider<object> provider = null!;
        
        // Act
        Action act = () => new ObjectStore<object>(provider);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("provider", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithValidProvider_SetsName()
    {
        // Arrange
        var provider = new Mock<ITransactionProvider<object>>().Object;

        // Act
        var objectStoreSet = new ObjectStore<object>(provider);

        // Assert
        Assert.Equal("Object", objectStoreSet.Name);
    }

    [Fact]
    public void Where_WithNull_ThrowsArgumentNullException()
    {
        // Arrange
        var provider = new Mock<ITransactionProvider<object>>().Object;
        var objectStoreSet = new ObjectStore<object>(provider);

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
        var objectStoreSet = new ObjectStore<object>(providerMock.Object);
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
        var objectStoreSet = new ObjectStore<object>(provider);
        Expression<Func<object, bool>> expression = _ => true;

        // Act
        var result = objectStoreSet.Where(expression);
        
        // Assert
        Assert.Same(objectStoreSet, result);
    }
}