using BlazorIndexedOrm.Core.UnitTests.Mock.Transaction.JsExpression.MemberCallTranslation;

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
    public void AddCustomMemberTranslator_WhenCalledGenericallyWIthExistingMember_ReplacesMemberTranslators()
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
}