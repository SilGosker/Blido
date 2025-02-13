using BlazorIndexedOrm.Core.UnitTests.Mock.ObjectStore.ObjectStore;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MemberTranslation;
using Moq;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression;

public class JsMemberExpressionBuilderTests
{
    [Fact]
    public void AppendMember_WithPropertyExpression_AppendsMemberName()
    {
        // Arrange
        var sb = new StringBuilder();
        var objectStore = new MockObjectStore();
        var expression = Expression.Property(Expression.Constant(objectStore), "Name");
        var processExpression = new ProcessExpression(_ => { });
        var memberTranslatorFactory = new Mock<IMemberTranslatorFactory>();
        // Act
        JsMemberExpressionBuilder.AppendMember(sb, memberTranslatorFactory.Object, expression, processExpression);

        // Assert
        Assert.Equal(".Name", sb.ToString());
    }

    [Fact]
    public void AppendMember_WithAttributeExpression_AppendsAttributeValue()
    {
        // Arrange
        var sb = new StringBuilder();
        var objectStore = new MockObjectStoreWithFieldAttribute();
        var expression = Expression.Property(Expression.Constant(objectStore), "Name");
        var processExpression = new ProcessExpression(_ => { });
        var memberTranslatorFactory = new Mock<IMemberTranslatorFactory>();

        // Act
        JsMemberExpressionBuilder.AppendMember(sb, memberTranslatorFactory.Object, expression, processExpression);

        // Assert
        Assert.Equal(".__name", sb.ToString());
    }

    [Fact]
    public void AppendMember_WithNativeProperty_AppendsValueFromMemberTranslator()
    {
        // Arrange
        var sb = new StringBuilder();
        var objectStore = new MockObjectStore();
        var expression = Expression.Property(Expression.Constant(objectStore), "Name");
        var processExpression = new ProcessExpression(_ => { });
        var memberTranslatorFactory = new Mock<IMemberTranslatorFactory>();
        TranslateMember translateMember = (builder, _, _) => builder.Append(".Name2");
        memberTranslatorFactory.Setup(x => x.TryGetValue(It.IsAny<MemberInfo>(), out translateMember!)).Returns(true);

        // Act
        JsMemberExpressionBuilder.AppendMember(sb, memberTranslatorFactory.Object, expression, processExpression);

        // Assert
        Assert.Equal(".Name2", sb.ToString());
    }
}