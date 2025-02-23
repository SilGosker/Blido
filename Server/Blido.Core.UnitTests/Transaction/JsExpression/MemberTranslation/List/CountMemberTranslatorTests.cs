using System.Linq.Expressions;
using System.Text;

namespace Blido.Core.Transaction.JsExpression.MemberTranslation.List;

public class CountMemberTranslatorTests
{
    [Fact]
    public void SupportedMembers_DoesNotContainNull()
    {
        // Arrange
        var members = CountMemberTranslator.SupportedMembers;

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
        var expression = Expression.PropertyOrField(Expression.Constant(new List<int>()), nameof(List<int>.Count));
        ProcessExpression processExpression = (next) =>
        {
            if (next is ConstantExpression { Value: List<int> s })
            {
                builder.Append('[');
                builder.Append(string.Join(',', s));
                builder.Append(']');
            }
        };
        // Act
        CountMemberTranslator.TranslateMember(builder, expression, processExpression);

        // Assert
        Assert.Equal("[].length", builder.ToString());
    }
}