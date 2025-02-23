using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.MemberTranslation.String;

public class LengthMemberTranslatorTests
{
    [Fact]
    public void SupportedMembers_DoesNotContainNull()
    {
        // Arrange
        var members = LengthMemberTranslator.SupportedMembers;

        // Act
        var containsNull = members.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMember_AppendsLengthProperty()
    {
        // Arrange
        var builder = new StringBuilder();
        var expression = Expression.PropertyOrField(Expression.Constant("test"), nameof(string.Length));
        ProcessExpression processExpression = (next) =>
        {
            if (next is ConstantExpression { Value: string s})
            {
                builder.Append('\"');
                builder.Append(s);
                builder.Append('\"');
            }
        };

        // Act
        LengthMemberTranslator.TranslateMember(builder, expression, processExpression);
        
        // Assert
        Assert.Equal("\"test\".length", builder.ToString());
    }
}