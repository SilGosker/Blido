using System.Linq.Expressions;
using System.Reflection.PortableExecutable;
using Blido.Core.UnitTests.Mock.Transaction.JsExpression.BinaryTranslation;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation;

public class JsBinaryTranslatorFactoryTests
{
    [Fact]
    public void AddCustomBinaryTranslator_WhenBinaryDidNotExist_AddsCustomBinaryTranslator()
    {
        // Arrange
        var factory = new JsBinaryTranslatorFactory();
        int supportedBinaryCount = factory.Count;
        var binary = MockUnsupportedBinaryTranslator.SupportedHashes[0];

        // Act
        factory.AddCustomBinaryTranslator(MockUnsupportedBinaryTranslator.TryMatchBinary, binary, MockUnsupportedBinaryTranslator.TranslateBinary);

        // Assert
        Assert.Equal(supportedBinaryCount + 1, factory.Count);
        Assert.True(factory.TryGetValue(binary, out var translateBinary));
        Assert.Same(MockUnsupportedBinaryTranslator.TranslateBinary, translateBinary);
    }

    [Fact]
    public void AddCustomBinaryTranslator_WhenCalledWithGeneric_AddsCustomBinaryTranslator()
    {
        // Arrange
        var factory = new JsBinaryTranslatorFactory();
        var binary = MockUnsupportedBinaryTranslator.SupportedHashes[0];
        int supportedBinaryCount = factory.Count;
        // Act
        factory.AddCustomBinaryTranslator<MockUnsupportedBinaryTranslator>();

        // Assert
        Assert.True(factory.TryGetValue(binary, out var translateBinary));
        Assert.Equal(MockUnsupportedBinaryTranslator.TranslateBinary, translateBinary);
        Assert.Equal(supportedBinaryCount + MockUnsupportedBinaryTranslator.SupportedHashes.Length, factory.Count);
    }

    [Fact]
    public void AddCustomBinaryTranslator_WhenCalledWithExistingBinary_ReplacesBinaryTranslator()
    {
        // Arrange
        var factory = new JsBinaryTranslatorFactory();
        var binaryHash = MockSupportedBinaryTranslator.SupportedHashes[0];
        int supportedBinaryCount = factory.Count;

        // Act
        factory.AddCustomBinaryTranslator(MockSupportedBinaryTranslator.TryMatchBinary, binaryHash, MockSupportedBinaryTranslator.TranslateBinary);
        
        // Assert
        Assert.True(factory.TryGetValue(binaryHash, out var translateBinary));
        Assert.Equal(MockSupportedBinaryTranslator.TranslateBinary, translateBinary);
        Assert.Equal(supportedBinaryCount, factory.Count);
    }

    [Fact]
    public void AddCustomBinaryTranslator_WhenCalledGenericallyWithExistingBinaryTranslator_ReplacesBinaryTranslators()
    {
        // Arrange
        var factory = new JsBinaryTranslatorFactory();
        var binaryHash = MockSupportedBinaryTranslator.SupportedHashes[0];
        int supportedBinaryCount = factory.Count;

        // Act
        factory.AddCustomBinaryTranslator<MockSupportedBinaryTranslator>();

        // Assert
        Assert.True(factory.TryGetValue(binaryHash, out var translateBinary));
        Assert.Equal(MockSupportedBinaryTranslator.TranslateBinary, translateBinary);
        Assert.Equal(supportedBinaryCount, factory.Count);
    }

    [Fact]
    public void ContainsKey_WhenKeyExists_ReturnsTrue()
    {
        // Arrange
        var factory = new JsBinaryTranslatorFactory();
        var binaryHash = MockSupportedBinaryTranslator.SupportedHashes[0];
        factory.AddCustomBinaryTranslator<MockSupportedBinaryTranslator>();
        
        // Act
        var result = factory.ContainsKey(binaryHash);
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ContainsKey_WhenKeyDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var factory = new JsBinaryTranslatorFactory();
        var binaryHash = MockUnsupportedBinaryTranslator.SupportedHashes[0];

        // Act
        var result = factory.ContainsKey(binaryHash);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryGetValue_WhenKeyExists_ReturnsTrue()
    {
        // Arrange
        var factory = new JsBinaryTranslatorFactory();
        var binaryHash = MockSupportedBinaryTranslator.SupportedHashes[0];
        factory.AddCustomBinaryTranslator<MockSupportedBinaryTranslator>();

        // Act
        var result = factory.TryGetValue(binaryHash, out var translateBinary);

        // Assert
        Assert.True(result);
        Assert.Equal(MockSupportedBinaryTranslator.TranslateBinary, translateBinary);
    }

    [Fact]
    public void TryGetValue_WhenKeyDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var factory = new JsBinaryTranslatorFactory();
        var binaryHash = MockUnsupportedBinaryTranslator.SupportedHashes[0];
        // Act
        var result = factory.TryGetValue(binaryHash, out var translateBinary);
        // Assert
        Assert.False(result);
        Assert.Null(translateBinary);
    }

    [Fact]
    public void Index_WhenKeyExists_ReturnsValue()
    {
        // Arrange
        var factory = new JsBinaryTranslatorFactory();
        var binaryHash = MockSupportedBinaryTranslator.SupportedHashes[0];
        factory.AddCustomBinaryTranslator<MockSupportedBinaryTranslator>();
        // Act
        var translateBinary = factory[binaryHash];
        // Assert
        Assert.Equal(MockSupportedBinaryTranslator.TranslateBinary, translateBinary);
    }

    [Fact]
    public void Index_WhenKeyDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var factory = new JsBinaryTranslatorFactory();
        var binaryHash = MockUnsupportedBinaryTranslator.SupportedHashes[0];
        // Act
        Action act = () => _ = factory[binaryHash];
        // Assert
        Assert.Throws<KeyNotFoundException>(act);
    }

    [Fact]
    public void Keys_ReturnKeysEnumerable()
    {
        // Arrange
        var factory = new JsBinaryTranslatorFactory();
        factory.AddCustomBinaryTranslator<MockSupportedBinaryTranslator>();
        
        // Act
        var keys = factory.Keys;
        
        // Assert
        Assert.NotNull(keys);
    }

    [Fact]
    public void Values_ReturnValuesEnumerable()
    {
        // Arrange
        var factory = new JsBinaryTranslatorFactory();
        factory.AddCustomBinaryTranslator<MockSupportedBinaryTranslator>();

        // Act
        var values = factory.Values;

        // Assert
        Assert.NotNull(values);
    }

    [Fact]
    public void GetEnumerator_WhenCalled_ReturnsEnumerator()
    {
        // Arrange
        var factory = new JsBinaryTranslatorFactory();
        factory.AddCustomBinaryTranslator<MockSupportedBinaryTranslator>();
        
        // Act
        var enumerator = factory.GetEnumerator();
        
        // Assert
        Assert.NotNull(enumerator);
    }

    [Fact]
    public void Count_ReturnsCount()
    {
        // Arrange
        var factory = new JsBinaryTranslatorFactory();
        factory.AddCustomBinaryTranslator<MockSupportedBinaryTranslator>();

        // Act
        var count = factory.Count;

        // Assert
        Assert.True(count > 0);
    }
}