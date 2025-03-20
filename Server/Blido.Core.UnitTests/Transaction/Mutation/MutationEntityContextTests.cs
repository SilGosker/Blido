namespace Blido.Core.Transaction.Mutation;

public class MutationEntityContextTests
{
    [Fact]
    public void Constructor_WhenEntityIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        object entity = null!;
        var state = MutationState.Added;

        // Act
        Action act = () => new MutationEntityContext(entity, state);
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("entity", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenStateIsInvalid_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var entity = new object();
        var state = (MutationState)int.MaxValue;
        // Act
        Action act = () => new MutationEntityContext(entity, state);

        // Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(act);
        Assert.Equal("state", exception.ParamName);
    }

    [Fact]
    public void Constructor_InitializesBeforeChange()
    {
        // Arrange
        var entity = new object();
        var state = MutationState.Added;

        // Act
        var context = new MutationEntityContext(entity, state);
        
        // Assert
        Assert.Same(entity, context.BeforeChange);
    }

    [Fact]
    public void Constructor_InitializesState()
    {
        // Arrange
        var entity = new object();
        var state = MutationState.Added;

        // Act
        var context = new MutationEntityContext(entity, state);
        
        // Assert
        Assert.Equal(state, context.State);
    }

    [Fact]
    public void Constructor_InitializesPrimaryKeys()
    {
        // Arrange
        var entity = new { Id = 1 };
        var state = MutationState.Added;

        // Act
        var context = new MutationEntityContext(entity, state);

        // Assert
        Assert.Single(context.PrimaryKeys);
        Assert.Equal("Id", context.PrimaryKeys.First().Name);
    }

    [Theory]
    [InlineData(MutationState.Added)]
    [InlineData(MutationState.Deleted)]
    [InlineData(MutationState.Modified)]
    public void StateMethodName_ReturnsJsMethodName(MutationState state)
    {
        var entity = new { Id = 1 };
        var mutationContext = new MutationEntityContext(entity, state);

        // Act
        var methodName = mutationContext.StateMethodName;

        // Assert
        Assert.True(JsMethodNames.MaterializerMethodNames.TryGetValue(methodName, out _));
    }
}