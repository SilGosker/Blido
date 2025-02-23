using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.MemberTranslation.Anonymous;

public class AnonymousMemberTranslatorTests
{
    [Fact]
    public void SupportedMembers_ShouldNotContainNull()
    {
        // Arrange
        MemberInfo[] supportedMembers = AnonymousMemberTranslator.SupportedMembers;

        // Act
        bool containsNull = supportedMembers.Contains(null);
        
        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMember_WhenMemberIsProperty_ShouldAppendEscapedValue()
    {
        // Arrange
        StringBuilder builder = new();
        var expression = Expression.Property(Expression.Constant(new { x = 1 }), "x");
        ProcessExpression processExpression = (next) =>
        {
        };

        // Act
        AnonymousMemberTranslator.TranslateMember(builder, expression, processExpression);
        
        // Assert
        Assert.Equal("1", builder.ToString());
    }
}