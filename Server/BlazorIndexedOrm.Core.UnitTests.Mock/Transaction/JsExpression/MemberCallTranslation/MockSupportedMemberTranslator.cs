using System.Reflection;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MemberTranslation;

namespace BlazorIndexedOrm.Core.UnitTests.Mock.Transaction.JsExpression.MemberCallTranslation;

public class MockSupportedMemberTranslator : IMemberTranslator
{
    public static TranslateMember TranslateMember => (builder, expression, processNext) =>
    {
        builder.Append(".length2");
    };
    public static MemberInfo[] SupportedMembers => typeof(string).GetMember(nameof(string.Length));
}