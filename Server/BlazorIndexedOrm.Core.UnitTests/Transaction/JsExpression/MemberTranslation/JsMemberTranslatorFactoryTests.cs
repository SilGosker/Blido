using BlazorIndexedOrm.Core.UnitTests.Mock.Transaction.JsExpression.MemberCallTranslation;
using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MemberTranslation;

public class JsMemberTranslatorFactoryTests
{
    [Fact]
    public void AddCustomMemberTranslator_WhenMemberDidNotExist_AddsCustomMemberTranslator()
    {
        // Arrange
        var factory = new JsMemberTranslatorFactory();
        int supportedMemberCount = factory.Count;
        var member = MockUnsupportedMemberTranslator.SupportedMembers[0];

        // Act
        factory.AddCustomMemberTranslator(member, MockUnsupportedMemberTranslator.TranslateMember);

        // Assert
        Assert.Equal(supportedMemberCount + 1, factory.Count);
        Assert.True(factory.TryGetValue(member, out var translateMember));
        Assert.Same(MockUnsupportedMemberTranslator.TranslateMember, translateMember);
    }

    [Fact]
    public void AddCustomMemberTranslator_WhenCalledWithGeneric_AddsCustomMemberTranslator()
    {
        // Arrange
        var factory = new JsMemberTranslatorFactory();
        var member = MockUnsupportedMemberTranslator.SupportedMembers[0];
        int supportedMemberCount = factory.Count;

        // Act
        factory.AddCustomMemberTranslator<MockUnsupportedMemberTranslator>();
        
        // Assert
        Assert.True(factory.TryGetValue(member, out var translateMember));
        Assert.Equal(MockUnsupportedMemberTranslator.TranslateMember, translateMember);
        Assert.Equal(supportedMemberCount + MockUnsupportedMemberTranslator.SupportedMembers.Length, factory.Count);
    }

    [Fact]
    public void AddCustomMemberTranslator_WhenCalledWithExistingMember_ReplacesMemberTranslator()
    {
        // Arrange
        var factory = new JsMemberTranslatorFactory();
        var member = MockSupportedMemberTranslator.SupportedMembers[0];
        int supportedMemberCount = factory.Count;

        // Act
        factory.AddCustomMemberTranslator(member, MockSupportedMemberTranslator.TranslateMember);
        
        // Assert
        Assert.True(factory.TryGetValue(member, out var translateMember));
        Assert.Equal(MockSupportedMemberTranslator.TranslateMember, translateMember);
        Assert.Equal(supportedMemberCount, factory.Count);
    }

    [Fact]
    public void AddCustomMemberTranslator_WhenCalledGenericallyWithExistingMember_ReplacesMemberTranslators()
    {
        // Arrange
        var factory = new JsMemberTranslatorFactory();
        var member = MockSupportedMemberTranslator.SupportedMembers[0];
        int supportedMemberCount = factory.Count;

        // Act
        factory.AddCustomMemberTranslator<MockSupportedMemberTranslator>();
        
        // Assert
        Assert.True(factory.TryGetValue(member, out var translateMember));
        Assert.Equal(MockSupportedMemberTranslator.TranslateMember, translateMember);
        Assert.Equal(supportedMemberCount, factory.Count);
    }


    [Fact]
    public void ContainsKey_WhenKeyExists_ReturnsTrue()
    {
        // Arrange
        var factory = new JsMemberTranslatorFactory();
        var method = MockSupportedMemberTranslator.SupportedMembers[0];
        factory.AddCustomMemberTranslator(method, MockSupportedMemberTranslator.TranslateMember);

        // Act
        bool containsKey = factory.ContainsKey(method);

        // Assert
        Assert.True(containsKey);
    }

    [Fact]
    public void ContainsKey_WhenKeyDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var factory = new JsMemberTranslatorFactory();
        var method = MockUnsupportedMemberTranslator.SupportedMembers[0];

        // Act
        bool containsKey = factory.ContainsKey(method);

        // Assert
        Assert.False(containsKey);
    }

    [Fact]
    public void ContainsKey_WhenKeyIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var factory = new JsMemberTranslatorFactory();
        MemberInfo? method = null;

        // Act
        Action act = () => factory.ContainsKey(method!);

        // Assert
        Assert.Throws<ArgumentNullException>(act);
    }

    [Fact]
    public void TryGetValue_WhenKeyExists_ReturnsTrue()
    {
        // Arrange
        var factory = new JsMemberTranslatorFactory();
        var method = MockSupportedMemberTranslator.SupportedMembers[0];
        factory.AddCustomMemberTranslator(method, MockSupportedMemberTranslator.TranslateMember);

        // Act
        bool tryGetValue = factory.TryGetValue(method, out var translateMethod);

        // Assert
        Assert.True(tryGetValue);
        Assert.Equal(MockSupportedMemberTranslator.TranslateMember, translateMethod);
    }

    [Fact]
    public void TryGetValue_WhenKeyDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var factory = new JsMemberTranslatorFactory();
        var method = MockUnsupportedMemberTranslator.SupportedMembers[0];

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
        var factory = new JsMemberTranslatorFactory();
        var method = MockSupportedMemberTranslator.SupportedMembers[0];
        factory.AddCustomMemberTranslator(method, MockSupportedMemberTranslator.TranslateMember);

        // Act
        var translateMethod = factory[method];

        // Assert
        Assert.Equal(MockSupportedMemberTranslator.TranslateMember, translateMethod);
    }

    [Fact]
    public void Index_WhenKeyDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var factory = new JsMemberTranslatorFactory();
        var method = MockUnsupportedMemberTranslator.SupportedMembers[0];

        // Act
        var act = () => factory[method];

        // Assert
        Assert.Throws<KeyNotFoundException>(act);
    }

    [Fact]
    public void Keys_ReturnsKeyEnumerable()
    {
        // Arrange
        var factory = new JsMemberTranslatorFactory();

        // Act
        var keys = factory.Keys;

        // Assert
        Assert.NotNull(keys);
    }

    [Fact]
    public void Values_ReturnsValueEnumerable()
    {
        // Arrange
        var factory = new JsMemberTranslatorFactory();
        // Act
        var values = factory.Values;

        // Assert
        Assert.NotNull(values);
    }

    [Fact]
    public void GetEnumerator_ReturnsEnumerator()
    {
        // Arrange
        var factory = new JsMemberTranslatorFactory();
        var method = MockSupportedMemberTranslator.SupportedMembers[0];
        factory.AddCustomMemberTranslator(method, MockSupportedMemberTranslator.TranslateMember);

        // Act
        var enumerator = factory.GetEnumerator();

        // Assert
        Assert.NotNull(enumerator);
    }

    [Fact]
    public void Count_ReturnsCount()
    {
        // Arrange
        var factory = new JsMemberTranslatorFactory();

        // Act
        int count = factory.Count;

        // Assert
        Assert.True(count > 0);
    }
}