using System.Numerics;

namespace Blido.Core.Helpers;

public class NumberHelperTests
{
    [Fact]
    public void NumberTypes_ShouldNotBeNull()
    {
        // Arrange
        var numberTypes = NumberHelper.NumberTypes;

        // Assert
        Assert.NotNull(numberTypes);
    }

    [Fact]
    public void NumberTypes_ShouldNotContainNull()
    {
        // Arrange
        var numberTypes = NumberHelper.NumberTypes;

        // Assert
        Assert.DoesNotContain(null, numberTypes);
    }

    [Fact]
    public void NumberTypes_ShouldImplementINumber()
    {
        // Arrange
        var numberTypes = NumberHelper.NumberTypes;

        // Assert
        Assert.All(numberTypes, type => Assert.Contains(type.GetInterfaces(), x => x.IsGenericType
            && x.GetGenericTypeDefinition() == typeof(INumber<>)));
    }
}