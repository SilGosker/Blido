using System.Reflection;

namespace Blido.Core.Transaction.Mutation.KeyGeneration;

public class KeyGeneratorFactoryTests
{
    [Fact]
    public void TryGetValue_WhenKeyIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var factory = new KeyGeneratorFactory();
        PropertyInfo key = null!;

        // Act
        Action action = () => factory.TryGetValue(key, out _);
        
        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact]
    public void TryGetValue_WhenKeyDoesNotExists_ReturnsFalse()
    {
        // Arrange
        var factory = new KeyGeneratorFactory();
        var key = new { id = "" }.GetType().GetProperty("id")!;
        
        // Act
        var result = factory.TryGetValue(key, out var generateKeyDelegate);

        // Assert
        Assert.False(result);
        Assert.Null(generateKeyDelegate);
    }

    [Fact]
    public void TryGetValue_WhenKeyExists_ReturnsTrue()
    {
        // Arrange
        var factory = new KeyGeneratorFactory();
        var key = new { id = "" }.GetType().GetProperty("id")!;
        GenerateKeyDelegate expected = (_, _) => { };
        factory.AddOrReplace(key.PropertyType, expected);

        // Act
        var result = factory.TryGetValue(key, out var generateKeyDelegate);
        
        // Assert
        Assert.True(result);
        Assert.Same(expected, generateKeyDelegate);
    }

    [Fact]
    public void AddOrReplace_WhenKeyGeneratorIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var factory = new KeyGeneratorFactory();
        Type key = null!;
        GenerateKeyDelegate generator = (_, _) => { };

        // Act
        Action action = () => factory.AddOrReplace(key, generator);
        
        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact]
    public void AddOrReplace_WhenInfoIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var factory = new KeyGeneratorFactory();
        Type key = typeof(string);
        GenerateKeyDelegate generator = null!;

        // Act
        Action action = () => factory.AddOrReplace(key, generator);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact]
    public void AddOrReplace_WhenKeyExists_ReplacesKey()
    {
        // Arrange
        var factory = new KeyGeneratorFactory();
        var key = new { id = "" }.GetType().GetProperty("id")!;
        GenerateKeyDelegate expected = (_, _) => { };
        factory.AddOrReplace(key.PropertyType, expected);
        GenerateKeyDelegate newGenerator = (_, _) => { };

        // Act
        factory.AddOrReplace(key.PropertyType, newGenerator);
        var result = factory.TryGetValue(key, out var generateKeyDelegate);
        
        // Assert
        Assert.True(result);
        Assert.Same(newGenerator, generateKeyDelegate);
    }

    [Fact]
    public void AddOrReplace_WhenKeyDoesNotExists_AddsKey()
    {
        // Arrange
        var factory = new KeyGeneratorFactory();
        var key = new { id = "" }.GetType().GetProperty("id")!;
        GenerateKeyDelegate expected = (_, _) => { };
        // Act
        factory.AddOrReplace(key.PropertyType, expected);
        var result = factory.TryGetValue(key, out var generateKeyDelegate);

        // Assert
        Assert.True(result);
        Assert.Same(expected, generateKeyDelegate);
    }

    [Fact]
    public void AddOrReplace_WhenStateIsConfirmed_ThrowsInvalidOperationException()
    {
        // Arrange
        var factory = new KeyGeneratorFactory();
        factory.AddOrReplace(typeof(string), (_, _) => { });
        ((IKeyGeneratorFactory)factory).Confirm();

        // Act
        Action action = () => factory.AddOrReplace(typeof(int), (_, _) => { });
        
        // Assert
        Assert.Throws<InvalidOperationException>(action);
    }
}