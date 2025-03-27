namespace Blido.Core.Transaction.Mutation.Arguments;

public class MutateBatchObjectStoreArgumentsTests
{
    [Theory]
    [InlineData("objectStore")]
    [InlineData("")]
    [InlineData("\t \t ")]
    [InlineData("null")]
    public void Properties_ShouldSetValues(string objectStore)
    {
        // Arrange
        var mutateBatchObjectStoreArguments = new MutateBatchObjectStoreArguments();

        // Act
        mutateBatchObjectStoreArguments.Entities = null!;
        mutateBatchObjectStoreArguments.ObjectStore = objectStore;

        // Assert
        Assert.Null(mutateBatchObjectStoreArguments.Entities);
        Assert.Equal(objectStore, mutateBatchObjectStoreArguments.ObjectStore);
    }
}