namespace Blido.Core.Transaction.Mutation.Arguments;

public class MutateBatchEntityArgumentsTests
{
    [Theory]
    [InlineData("""
                {
                "Property": "Value"
                }
                """, MutationState.Added)]
    [InlineData("", MutationState.Modified)]
    [InlineData(null, (MutationState)0)]
    [InlineData("\t  \t", (MutationState)int.MaxValue)]
    public void Properties_ShouldSetValues(string json, MutationState state)
    {
        // Arrange
        var entity = new MutateBatchEntityArguments();

        // Act
        entity.Entity = json;
        entity.State = state;

        // Assert
        Assert.Equal(json, entity.Entity);
        Assert.Equal(state, entity.State);
    }
}