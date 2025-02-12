using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MemberTranslation;

public interface IMemberTranslatorFactory : IReadOnlyDictionary<MemberInfo, TranslateMember>
{
    public void AddCustomMemberTranslator<TTranslator>() where TTranslator : IMemberTranslator;
    public void AddCustomMemberTranslator(MemberInfo method, TranslateMember translateMethod);
    internal void Confirm();
}