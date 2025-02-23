using Blido.Core.Helpers;
using Blido.Core.Transaction.JsExpression.MethodCallTranslation;
using System.Collections;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Blido.Core.Transaction.JsExpression.MemberTranslation;

public class JsMemberTranslatorFactory : IMemberTranslatorFactory
{
    private IReadOnlyDictionary<MemberInfo, TranslateMember> _translators;

    public JsMemberTranslatorFactory()
    {
        var translators = new Dictionary<MemberInfo, TranslateMember>(new MemberInfoEqualityComparer());
        var blazorIndexedOrmTypes = Assembly.GetExecutingAssembly().GetTypes();

        foreach (Type type in blazorIndexedOrmTypes.Where(x => x.IsClass && x.GetInterface(nameof(IMemberTranslator)) != null))
        {
            if (type.GetProperty(nameof(IMemberTranslator.SupportedMembers))!.GetValue(null)
                is not MemberInfo[] supportedMembers) continue;

            if (type.GetProperty(nameof(IMemberTranslator.TranslateMember))!.GetValue(null)
                is not TranslateMember translateMember) continue;

            foreach (MemberInfo method in supportedMembers)
            {
                translators.Add(method, translateMember);
            }
        }

        _translators = translators;
    }
    public void AddCustomMemberTranslator<TTranslator>() where TTranslator : IMemberTranslator
    {
        foreach (MemberInfo supportedMember in TTranslator.SupportedMembers)
        {
            AddCustomMemberTranslator(supportedMember, TTranslator.TranslateMember);
        }
    }

    public void AddCustomMemberTranslator(MemberInfo member, TranslateMember translateMethod)
    {
        ThrowHelper.ThrowDictionaryIsNotReadonlyException(_translators, out Dictionary<MemberInfo, TranslateMember> dict);
        dict[member] = translateMethod;
    }

    public void Confirm()
    {
        _translators = _translators.ToFrozenDictionary();
    }

    public IEnumerator<KeyValuePair<MemberInfo, TranslateMember>> GetEnumerator()
    {
        return _translators.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count => _translators.Count;
    public bool ContainsKey(MemberInfo key)
    {
        return _translators.ContainsKey(key);
    }

    public bool TryGetValue(MemberInfo key, [MaybeNullWhen(false)] out TranslateMember value)
    {
        return _translators.TryGetValue(key, out value);
    }

    public TranslateMember this[MemberInfo key] => _translators[key];

    public IEnumerable<MemberInfo> Keys => _translators.Keys;
    public IEnumerable<TranslateMember> Values => _translators.Values;
}