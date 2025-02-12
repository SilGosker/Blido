using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MemberTranslation.Array;

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
        var expression = Expression.PropertyOrField(Expression.Constant(new int[0]), nameof(System.Array.Length));
        ProcessExpression processExpression = (next) =>
        {
            if (next is ConstantExpression { Value: int[] s })
            {
                builder.Append('[');
                builder.Append(string.Join(',', s));
                builder.Append(']');
            }
        };
        // Act
        LengthMemberTranslator.TranslateMember(builder, expression, processExpression);
        // Assert
        Assert.Equal("[].length", builder.ToString());
    }
}