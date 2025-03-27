using System.Text.Json.Serialization;

namespace Blido.Core.Transaction.Materialization;

public class MaterializationArgumentsSerializationContextTests
{
    [Fact]
    public void DefaultMaterializationArguments_ShouldBeSourceGenerated()
    {
        // Arrange
        var options = MaterializationArgumentsSerializationContext.Default.MaterializationArguments;
        
        // Assert
        Assert.NotNull(options);
    }
}