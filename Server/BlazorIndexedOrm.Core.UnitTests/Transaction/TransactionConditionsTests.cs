using System.Linq.Expressions;
using BlazorIndexedOrm.Core.Transaction;

namespace BlazorIndexedOrm.Core.UnitTests.Transaction;

public class TransactionConditionsTests
{
    [Fact]
    public void AddConditions_WithNullExpression_ThrowArgumentNullException()
    {
        // Arrange
        var conditions = new TransactionConditions<object>();
        Expression<Func<object, bool>> expression = null!;

        // Act
        Action act = () => conditions.AddCondition(expression);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("expression", exception.ParamName);
    }

    [Fact]
    public void FullFillsConditions_WithNullEntity_ThrowArgumentNullException()
    {
        // Arrange
        var conditions = new TransactionConditions<object>();
        object entity = null!;

        // Act
        Action act = () => conditions.FullFillsConditions(entity);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("entity", exception.ParamName);
    }

    [Fact]
    public void FullFillsConditions_WithoutConditions_ReturnsTrue()
    {
        // Arrange
        var conditions = new TransactionConditions<object>();
        var entity = new object();

        // Act
        var result = conditions.FullFillsConditions(entity);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void FullFillsConditions_WithMatchingConditions_ReturnsTrue()
    {
        // Arrange
        var conditions = new TransactionConditions<object>();
        conditions.AddCondition(e => true);
        var entity = new object();

        // Act
        var result = conditions.FullFillsConditions(entity);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void FullFillsConditions_WithNotMatchingConditions_ReturnsFalse()
    {
        // Arrange
        var conditions = new TransactionConditions<object>();
        conditions.AddCondition(e => false);
        var entity = new object();
        // Act
        var result = conditions.FullFillsConditions(entity);
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void FullFillsConditions_WithDependentConditions_ReturnsResult()
    {
        // Arrange
        var conditions = new TransactionConditions<string>();
        conditions.AddCondition(e => string.IsNullOrEmpty(e));
        conditions.AddCondition(e => e != "test");
        string string1 = "123";
        string string2 = string.Empty;

        // Act
        var result1 = conditions.FullFillsConditions(string1);
        var result2 = conditions.FullFillsConditions(string2);

        // Assert
        Assert.False(result1);
        Assert.True(result2);
    }
}