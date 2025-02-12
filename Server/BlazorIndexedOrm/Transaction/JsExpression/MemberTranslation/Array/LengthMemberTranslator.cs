using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MemberTranslation.Array;

public class LengthMemberTranslator : IMemberTranslator
{
    public static TranslateMember TranslateMember => (builder, expression, processNext) =>
    {
        processNext(expression.Expression!);
        builder.Append(".length");
    };

    public static MemberInfo[] SupportedMembers => new[]
    {
        typeof(System.Array).GetProperty("Length")!
    };
}