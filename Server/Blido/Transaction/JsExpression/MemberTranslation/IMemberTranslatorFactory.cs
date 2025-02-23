using System.Reflection;

namespace Blido.Core.Transaction.JsExpression.MemberTranslation;

public interface IMemberTranslatorFactory : IReadOnlyDictionary<MemberInfo, TranslateMember>
{
    public void AddCustomMemberTranslator<TTranslator>() where TTranslator : IMemberTranslator;
    public void AddCustomMemberTranslator(MemberInfo method, TranslateMember translateMethod);
    internal void Confirm();
}