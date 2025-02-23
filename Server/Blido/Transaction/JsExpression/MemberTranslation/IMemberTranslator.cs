using System.Reflection;

namespace Blido.Core.Transaction.JsExpression.MemberTranslation;

public interface IMemberTranslator
{
    public static abstract TranslateMember TranslateMember { get; }
    public static abstract MemberInfo[] SupportedMembers { get; }
}