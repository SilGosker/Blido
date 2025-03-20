namespace Blido.Core.Transaction.Mutation;

public class MutationEntityContextTests
{
    [Fact]
    public void Insert_WhenEntityIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        object entity = null!;

        // Act
        Action act = () => MutationEntityContext.Insert(entity);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("entity", exception.ParamName);
    }

    [Fact]
    public void Insert_InitializesBeforeChange()
    {
        // Arrange
        var entity = new object();

        // Act
        var context = MutationEntityContext.Insert(entity);
        
        // Assert
        Assert.Same(entity, context.BeforeChange);
    }

    [Fact]
    public void Insert_InitializesState()
    {
        // Arrange
        var entity = new object();
        var state = MutationState.Added;

        // Act
        var context = MutationEntityContext.Insert(entity);
        
        // Assert
        Assert.Equal(state, context.State);
    }

    [Fact]
    public void Insert_InitializesPrimaryKeys()
    {
        // Arrange
        var entity = new { Id = 1 };

        // Act
        var context = MutationEntityContext.Insert(entity);

        // Assert
        Assert.Single(context.PrimaryKeys);
        Assert.Equal("Id", context.PrimaryKeys.First().Name);
    }

    [Fact]
    public void Update_WhenEntityIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        object entity = null!;

        // Act
        Action act = () => MutationEntityContext.Update(entity);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("entity", exception.ParamName);
    }

    [Fact]
    public void Update_InitializesBeforeChange()
    {
        // Arrange
        var entity = new object();

        // Act
        var context = MutationEntityContext.Update(entity);
        
        // Assert
        Assert.Same(entity, context.BeforeChange);
    }

    [Fact]
    public void Update_InitializesPrimaryKeys()
    {
        // Arrange
        var entity = new { Id = 1 };

        // Act
        var context = MutationEntityContext.Update(entity);
        
        // Assert
        Assert.Single(context.PrimaryKeys);
        Assert.Equal("Id", context.PrimaryKeys.First().Name);
    }


    [Fact]
    public void Delete_WhenEntityIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        object entity = null!;

        // Act
        Action act = () => MutationEntityContext.Delete(entity);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("entity", exception.ParamName);
    }

    [Fact]
    public void Delete_InitializesBeforeChange()
    {
        // Arrange
        var entity = new object();

        // Act
        var context = MutationEntityContext.Delete(entity);

        // Assert
        Assert.Same(entity, context.BeforeChange);
    }

    [Fact]
    public void Delete_InitializesPrimaryKeys()
    {
        // Arrange
        var entity = new { Id = 1 };

        // Act
        var context = MutationEntityContext.Delete(entity);

        // Assert
        Assert.Single(context.PrimaryKeys);
        Assert.Equal("Id", context.PrimaryKeys.First().Name);
    }

    [Fact]
    public void StateMethodName_WhenStateIsAdded_ReturnsInsert()
    {
        // Arrange
        var entity = new { Id = 1 };
        var mutationContext = MutationEntityContext.Insert(entity);

        // Act
        var methodName = mutationContext.StateMethodName;

        // Assert
        Assert.True(JsMethodNames.MaterializerMethodNames.TryGetValue(methodName, out string? name));
        Assert.Equal("blido.insert", name);
    }

    [Fact]
    public void StateMethodName_WhenStateIsModified_ReturnsUpdate()
    {
        // Arrange
        var entity = new { Id = 1 };
        var mutationContext = MutationEntityContext.Update(entity);
        
        // Act
        var methodName = mutationContext.StateMethodName;
        
        // Assert
        Assert.True(JsMethodNames.MaterializerMethodNames.TryGetValue(methodName, out string? name));
        Assert.Equal("blido.update", name);
    }

    [Fact]
    public void StateMethodName_WhenStateIsDeleted_ReturnsDelete()
    {
        // Arrange
        var entity = new { Id = 1 };
        var mutationContext = MutationEntityContext.Delete(entity);
        
        // Act
        var methodName = mutationContext.StateMethodName;

        // Assert
        Assert.True(JsMethodNames.MaterializerMethodNames.TryGetValue(methodName, out string? name));
        Assert.Equal("blido.delete", name);
    }
}