namespace Blido.Core.Transaction.Mutation.Arguments;

public class MutateBatchArgumentsSerializationContextTests
{
    [Fact]
    public void DefaultMutateBatchArguments_ShouldNotBeNull()
    {
        // Arrange
        var arguments = MutateBatchArgumentsSerializationContext.Default.MutateBatchArguments;

        // Assert
        Assert.NotNull(arguments);
    }
}