namespace Blido.Core.Transaction.Mutation.Arguments;

public class MutateArgumentsSerializationContextTests
{
    [Fact]
    public void DefaultMutateArguments_ShouldBeSourceGenerated()
    {
        // Arrange
        var options = MutateArgumentsSerializationContext.Default.MutateArguments;

        // Assert
        Assert.NotNull(options);
    }
}