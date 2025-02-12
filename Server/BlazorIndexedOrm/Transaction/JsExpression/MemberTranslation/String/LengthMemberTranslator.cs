using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MemberTranslation.String;

public class LengthMemberTranslator : IMemberTranslator
{
    public static TranslateMember TranslateMember => (builder, expression, processNext) =>
    {
        processNext(expression.Expression!);
        builder.Append(".length");
    };

    public static MemberInfo[] SupportedMembers => new[]
    {
        typeof(string).GetProperty(nameof(string.Length))!
    };
}