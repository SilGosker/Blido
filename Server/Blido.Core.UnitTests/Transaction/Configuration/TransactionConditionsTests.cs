using System.Linq.Expressions;

namespace Blido.Core.Transaction.Configuration;

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
    public void HasConditions_WithNoConditionsSupplied_ReturnsFalse()
    {
        // Arrange
        var conditions = new TransactionConditions<object>();

        // Act
        var result = conditions.HasConditions;

        // Assert
        Assert.False(result);

    }

    [Fact]
    public void HasConditions_WithConditionsSupplied_ReturnsTrue()
    {
        // Arrange
        var conditions = new TransactionConditions<object>();
        conditions.AddCondition(e => true);

        // Act
        var result = conditions.HasConditions;

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void Count_ReturnsAmountOfConditionsSupplied(int amount)
    {
        // Arrange
        var conditions = new TransactionConditions<object>();
        for (int i = 0; i < amount; i++)
        {
            conditions.AddCondition(e => true);
        }

        // Act
        var result = conditions.Count;

        // Assert
        Assert.Equal(amount, result);
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