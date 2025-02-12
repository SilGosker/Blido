using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MemberTranslation.List;

public class CountMemberTranslator : IMemberTranslator
{
    public static TranslateMember TranslateMember => (builder, expression, processNext) =>
    {
        processNext(expression.Expression!);
        builder.Append(".length");
    };

    public static MemberInfo[] SupportedMembers => new[]
    {
        typeof(List<>).GetProperty("Count")!
    };
}