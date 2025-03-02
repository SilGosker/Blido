using System.Text.Json.Serialization;

namespace Blido.Core.Transaction.Materialization;

public class MaterializationArgumentsSerializationContextTests
{
    [Fact]
    public void MaterializationArgumentsSerializationContext_JsonSerializerContext()
    {
        // Arrange
        var type = typeof(MaterializationArgumentsSerializationContext);
        
        // Act
        bool isSubTypeOf = type.IsSubclassOf(typeof(JsonSerializerContext));

        // Assert
        Assert.True(isSubTypeOf);
    }
}