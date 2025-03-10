namespace Blido.Core.Transaction.JsExpression.UnaryTranslation;

public class JsUnaryTranslatorFactoryTests
{
    [Fact]
    public void AddUnaryTranslator_WhenUnaryDidNotExist_AddsUnaryTranslator()
    {
        // Arrange
        var factory = new JsUnaryTranslatorFactory();
        int supportedUnaryCount = factory.Count;
        var unary = MockUnsupportedUnaryTranslator.SupportedUnaries[0];
        
        // Act
        factory.AddUnaryTranslator(unary, MockUnsupportedUnaryTranslator.TranslateUnary);
        
        // Assert
        Assert.Equal(supportedUnaryCount + 1, factory.Count);
        Assert.True(factory.TryGetValue(unary, out var translateMethod));
        Assert.Same(MockUnsupportedUnaryTranslator.TranslateUnary, translateMethod);
    }

    [Fact]
    public void AddUnaryTranslator_WhenCalledWithGeneric_AddsUnaryTranslator()
    {
        // Arrange
        var factory = new JsUnaryTranslatorFactory();
        var unary = MockUnsupportedUnaryTranslator.SupportedUnaries[0];
        int supportedUnaryCount = factory.Count;

        // Act
        factory.AddUnaryTranslator<MockUnsupportedUnaryTranslator>();

        // Assert
        Assert.True(factory.TryGetValue(unary, out var translateMethod));
        Assert.Equal(MockUnsupportedUnaryTranslator.TranslateUnary, translateMethod);
        Assert.Equal(supportedUnaryCount + MockUnsupportedUnaryTranslator.SupportedUnaries.Length, factory.Count);
    }

    [Fact]
    public void AddUnaryTranslator_WhenCalledWithExistingUnary_ReplacesUnaryTranslator()
    {
        // Arrange
        var factory = new JsUnaryTranslatorFactory();
        var unary = MockSupportedUnaryTranslator.SupportedUnaries[0];
        int supportedUnaryCount = factory.Count;
        
        // Act
        factory.AddUnaryTranslator(unary, MockSupportedUnaryTranslator.TranslateUnary);
        
        // Assert
        Assert.True(factory.TryGetValue(unary, out var translateMethod));
        Assert.Equal(MockSupportedUnaryTranslator.TranslateUnary, translateMethod);
        Assert.Equal(supportedUnaryCount, factory.Count);
    }

    [Fact]
    public void AddUnaryTranslator_WhenCalledGenericallyWIthExistingUnary_ReplacesUnaryTranslators()
    {
        // Arrange
        var factory = new JsUnaryTranslatorFactory();
        var unary = MockSupportedUnaryTranslator.SupportedUnaries[0];
        int supportedUnaryCount = factory.Count;
        
        // Act
        factory.AddUnaryTranslator<MockSupportedUnaryTranslator>();
        
        // Assert
        Assert.True(factory.TryGetValue(unary, out var translateMethod));
        Assert.Equal(MockSupportedUnaryTranslator.TranslateUnary, translateMethod);
        Assert.Equal(supportedUnaryCount, factory.Count);
    }

    [Fact]
    public void Confirm_WhenCalled_ConfirmsFactory()
    {
        // Arrange
        var factory = new JsUnaryTranslatorFactory();
        factory.AddUnaryTranslator<MockSupportedUnaryTranslator>();
        
        // Act
        factory.Confirm();
        
        // Assert
        Assert.Throws<InvalidOperationException>(() => factory.AddUnaryTranslator(MockSupportedUnaryTranslator.SupportedUnaries[0], MockSupportedUnaryTranslator.TranslateUnary));
    }

    [Fact]
    public void ContainsKey_WhenKeyExists_ReturnsTrue()
    {
        // Arrange
        var factory = new JsUnaryTranslatorFactory();
        var unary = MockSupportedUnaryTranslator.SupportedUnaries[0];
        factory.AddUnaryTranslator(unary, MockSupportedUnaryTranslator.TranslateUnary);

        // Act
        var result = factory.ContainsKey(unary);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ContainsKey_WhenKeyDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var factory = new JsUnaryTranslatorFactory();
        var unary = MockUnsupportedUnaryTranslator.SupportedUnaries[0];
        
        // Act
        var result = factory.ContainsKey(unary);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ContainsKey_WhenKeyIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var factory = new JsUnaryTranslatorFactory();
        
        // Act
        void Act() => factory.ContainsKey(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Act);
    }

    [Fact]
    public void TryGetValue_WhenKeyExists_ReturnsTrue()
    {
        // Arrange
        var factory = new JsUnaryTranslatorFactory();
        var unary = MockSupportedUnaryTranslator.SupportedUnaries[0];
        factory.AddUnaryTranslator(unary, MockSupportedUnaryTranslator.TranslateUnary);

        // Act
        var result = factory.TryGetValue(unary, out var translateMethod);
        
        // Assert
        Assert.True(result);
        Assert.Equal(MockSupportedUnaryTranslator.TranslateUnary, translateMethod);
    }

    [Fact]
    public void TryGetValue_WhenKeyDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var factory = new JsUnaryTranslatorFactory();
        var unary = MockUnsupportedUnaryTranslator.SupportedUnaries[0];
        
        // Act
        var result = factory.TryGetValue(unary, out var translateMethod);
        
        // Assert
        Assert.False(result);
        Assert.Null(translateMethod);
    }

    [Fact]
    public void TryGetValue_WhenKeyIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var factory = new JsUnaryTranslatorFactory();

        // Act
        void Act() => factory.TryGetValue(null!, out _);
        // Assert
        Assert.Throws<ArgumentNullException>(Act);
    }

    [Fact]
    public void GetEnumerator_WhenCalled_ReturnsEnumerator()
    {
        // Arrange
        var factory = new JsUnaryTranslatorFactory();
        factory.AddUnaryTranslator<MockSupportedUnaryTranslator>();

        // Act
        var enumerator = factory.GetEnumerator();

        // Assert
        Assert.NotNull(enumerator);
    }

    [Fact]
    public void Index_WhenKeyExists_ReturnsValue()
    {
        // Arrange
        var factory = new JsUnaryTranslatorFactory();
        var unary = MockSupportedUnaryTranslator.SupportedUnaries[0];
        factory.AddUnaryTranslator(unary, MockSupportedUnaryTranslator.TranslateUnary);

        // Act
        var translateMethod = factory[unary];

        // Assert
        Assert.Equal(MockSupportedUnaryTranslator.TranslateUnary, translateMethod);
    }

    [Fact]
    public void Index_WhenKeyDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var factory = new JsUnaryTranslatorFactory();
        var unary = MockUnsupportedUnaryTranslator.SupportedUnaries[0];
        
        // Act
        void Act() => _ = factory[unary];
        
        // Assert
        Assert.Throws<KeyNotFoundException>(Act);
    }

    [Fact]
    public void Index_WhenKeyIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var factory = new JsUnaryTranslatorFactory();

        // Act
        void Act() => _ = factory[null!];
        
        // Assert
        Assert.Throws<ArgumentNullException>(Act);
    }

    [Fact]
    public void Keys_WhenCalled_ReturnsKeyEnumerable()
    {
        // Arrange
        var factory = new JsUnaryTranslatorFactory();
        factory.AddUnaryTranslator<MockSupportedUnaryTranslator>();
        
        // Act
        var keys = factory.Keys;
        
        // Assert
        Assert.NotNull(keys);
    }

    [Fact]
    public void Values_WhenCalled_ReturnsValueEnumerable()
    {
        // Arrange
        var factory = new JsUnaryTranslatorFactory();
        factory.AddUnaryTranslator<MockSupportedUnaryTranslator>();
        
        // Act
        var values = factory.Values;
        
        // Assert
        Assert.NotNull(values);
    }

    [Fact]
    public void Count_WhenCalled_ReturnsCount()
    {
        // Arrange
        var factory = new JsUnaryTranslatorFactory();
        factory.AddUnaryTranslator<MockSupportedUnaryTranslator>();

        // Act
        var count = factory.Count;
        
        // Assert
        Assert.True(count > 0);
    }
}