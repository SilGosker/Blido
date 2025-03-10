using System.Reflection;
using Blido.Core.Transaction.JsExpression.MemberTranslation;

namespace Blido.Core.Transaction.JsExpression.MemberCallTranslation;

public class MockSupportedMemberTranslator : IMemberTranslator
{
    public static TranslateMember TranslateMember => (builder, expression, processNext) =>
    {
        builder.Append(".length2");
    };
    public static MemberInfo[] SupportedMembers => typeof(string).GetMember(nameof(string.Length));
}