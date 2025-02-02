using System.Reflection;
using BlazorIndexedOrm.Core.UnitTests.Mock.Transaction.JsExpression.MethodCallTranslation;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation;

public class JsMethodCallTranslatorFactoryTests
{
    [Fact]
    public void AddCustomMethodTranslator_WhenMethodDidNotExist_AddsCustomMethodTranslator()
    {
        // Arrange
        var factory = new JsMethodCallTranslatorFactory();
        int supportedMethodCount = factory.Count;
        var method = MockUnsupportedMethodCallTranslator.SupportedMethods[0];

        // Act
        factory.AddCustomMethodTranslator(method, MockUnsupportedMethodCallTranslator.TranslateMethodCall);
        
        // Assert
        Assert.Equal(supportedMethodCount + 1, factory.Count);
        Assert.True(factory.TryGetValue(method, out var translateMethod));
        Assert.Same(MockUnsupportedMethodCallTranslator.TranslateMethodCall, translateMethod);
    }

    [Fact]
    public void AddCustomMethodTranslator_WhenCalledWithGeneric_AddsCustomMethodTranslator()
    {
        // Arrange
        var factory = new JsMethodCallTranslatorFactory();
        var method = MockUnsupportedMethodCallTranslator.SupportedMethods[0];
        int supportedMethodCount = factory.Count;

        // Act
        factory.AddCustomMethodTranslator<MockUnsupportedMethodCallTranslator>();

        // Assert
        Assert.True(factory.TryGetValue(method, out var translateMethod));
        Assert.Equal(MockUnsupportedMethodCallTranslator.TranslateMethodCall, translateMethod);
        Assert.Equal(supportedMethodCount + MockUnsupportedMethodCallTranslator.SupportedMethods.Length, factory.Count);
    }

    [Fact]
    public void AddCustomMethodTranslator_WhenCalledWithExistingMethod_ReplacesMethodTranslator()
    {
        // Arrange
        var factory = new JsMethodCallTranslatorFactory();
        var method = MockContainsMethodCallTranslator.SupportedMethods[0];
        int supportedMethodCount = factory.Count;

        // Act
        factory.AddCustomMethodTranslator(method, MockContainsMethodCallTranslator.TranslateMethodCall);

        // Assert
        Assert.True(factory.TryGetValue(method, out var translateMethod));
        Assert.Equal(MockContainsMethodCallTranslator.TranslateMethodCall, translateMethod);
        Assert.Equal(supportedMethodCount, factory.Count);
    }

    [Fact]
    public void AddCustomMethodTranslator_WhenCalledGenericallyWIthExistingMethod_ReplacesMethodTranslators()
    {
        // Arrange
        var factory = new JsMethodCallTranslatorFactory();
        var method = MockContainsMethodCallTranslator.SupportedMethods[0];
        int supportedMethodCount = factory.Count;

        // Act
        factory.AddCustomMethodTranslator<MockContainsMethodCallTranslator>();

        // Assert
        Assert.True(factory.TryGetValue(method, out var translateMethod));
        Assert.Equal(MockContainsMethodCallTranslator.TranslateMethodCall, translateMethod);
        Assert.Equal(supportedMethodCount, factory.Count);
    }

    [Fact]
    public void Confirm_WhenCalled_FreezesDictionary()
    {
        // Arrange
        var factory = new JsMethodCallTranslatorFactory();
        var method = MockUnsupportedMethodCallTranslator.SupportedMethods[0];
        factory.AddCustomMethodTranslator(method, MockUnsupportedMethodCallTranslator.TranslateMethodCall);
        
        // Act
        ((IJsMethodCallTranslatorFactory)factory).Confirm();

        // Assert
        Assert.Throws<InvalidOperationException>(() => factory.AddCustomMethodTranslator(method, MockUnsupportedMethodCallTranslator.TranslateMethodCall));
    }

    [Fact]
    public void ContainsKey_WhenKeyExists_ReturnsTrue()
    {
        // Arrange
        var factory = new JsMethodCallTranslatorFactory();
        var method = MockContainsMethodCallTranslator.SupportedMethods[0];
        factory.AddCustomMethodTranslator(method, MockContainsMethodCallTranslator.TranslateMethodCall);

        // Act
        bool containsKey = factory.ContainsKey(method);
        
        // Assert
        Assert.True(containsKey);
    }

    [Fact]
    public void ContainsKey_WhenKeyDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var factory = new JsMethodCallTranslatorFactory();
        var method = MockUnsupportedMethodCallTranslator.SupportedMethods[0];

        // Act
        bool containsKey = factory.ContainsKey(method);

        // Assert
        Assert.False(containsKey);
    }

    [Fact]
    public void ContainsKey_WhenKeyIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var factory = new JsMethodCallTranslatorFactory();
        MethodInfo? method = null;

        // Act
        Action act = () => factory.ContainsKey(method!);
        
        // Assert
        Assert.Throws<ArgumentNullException>(act);
    }

    [Fact]
    public void TryGetValue_WhenKeyExists_ReturnsTrue()
    {
        // Arrange
        var factory = new JsMethodCallTranslatorFactory();
        var method = MockContainsMethodCallTranslator.SupportedMethods[0];
        factory.AddCustomMethodTranslator(method, MockContainsMethodCallTranslator.TranslateMethodCall);

        // Act
        bool tryGetValue = factory.TryGetValue(method, out var translateMethod);

        // Assert
        Assert.True(tryGetValue);
        Assert.Equal(MockContainsMethodCallTranslator.TranslateMethodCall, translateMethod);
    }

    [Fact]
    public void TryGetValue_WhenKeyDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var factory = new JsMethodCallTranslatorFactory();
        var method = MockUnsupportedMethodCallTranslator.SupportedMethods[0];

        // Act
        bool tryGetValue = factory.TryGetValue(method, out var translateMethod);

        // Assert
        Assert.False(tryGetValue);
        Assert.Null(translateMethod);
    }

    [Fact]
    public void Index_WhenKeyExists_ReturnsValue()
    {
        // Arrange
        var factory = new JsMethodCallTranslatorFactory();
        var method = MockContainsMethodCallTranslator.SupportedMethods[0];
        factory.AddCustomMethodTranslator(method, MockContainsMethodCallTranslator.TranslateMethodCall);

        // Act
        var translateMethod = factory[method];
        
        // Assert
        Assert.Equal(MockContainsMethodCallTranslator.TranslateMethodCall, translateMethod);
    }

    [Fact]
    public void Index_WhenKeyDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var factory = new JsMethodCallTranslatorFactory();
        var method = MockUnsupportedMethodCallTranslator.SupportedMethods[0];

        // Act
        var act = () => factory[method];
        
        // Assert
        Assert.Throws<KeyNotFoundException>(act);
    }

    [Fact]
    public void Keys_ReturnsKeyEnumerable()
    {
        // Arrange
        var factory = new JsMethodCallTranslatorFactory();

        // Act
        var keys = factory.Keys;
        
        // Assert
        Assert.NotNull(keys);
    }

    [Fact]
    public void Values_ReturnsValueEnumerable()
    {
        // Arrange
        var factory = new JsMethodCallTranslatorFactory();
        // Act
        var values = factory.Values;

        // Assert
        Assert.NotNull(values);
    }

    [Fact]
    public void GetEnumerator_ReturnsEnumerator()
    {
        // Arrange
        var factory = new JsMethodCallTranslatorFactory();
        var method = MockContainsMethodCallTranslator.SupportedMethods[0];
        factory.AddCustomMethodTranslator(method, MockContainsMethodCallTranslator.TranslateMethodCall);

        // Act
        var enumerator = factory.GetEnumerator();

        // Assert
        Assert.NotNull(enumerator);
    }

    [Fact]
    public void Count_ReturnsCount()
    {
        // Arrange
        var factory = new JsMethodCallTranslatorFactory();

        // Act
        int count = factory.Count;
        
        // Assert
        Assert.True(count > 0);
    }
}